using Microsoft.EntityFrameworkCore;
using WebBimba.Data;
using WebBimba.Data.Entities;
using WebBimba.Interfaces;
using WebBimba.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AppBimbaDbContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("MyConnectionDB")));

builder.Services.AddScoped<IImageWorker, ImageWorker>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Main}/{action=Index}/{id?}");

using (var serviceScope = app.Services.CreateScope())
{
    var context = serviceScope.ServiceProvider.GetService<AppBimbaDbContext>();
    var imageWorker = serviceScope.ServiceProvider.GetService<IImageWorker>();

    // Apply migrations if they are not applied
    context.Database.Migrate(); //автоматичний запуск міграцій на БД, якщо їх там немає

    if (!context.Categories.Any())
    {
        var imageName = imageWorker.Save("https://rivnepost.rv.ua/img/650/korisnoi-kovbasi-ne-buvae-hastroenterolohi-nazvali_20240612_4163.jpg");
        var kovbasa = new CategoryEntity
        {
            Name = "Ковбаси",
            Image = imageName,
            Description = "Тим часом відмовлятися від ковбаси повністю не обов’язково. " +
            "Важливо пам’ятати, що це делікатес, який можна вживати не більше 50 грамів на день."
        };

        imageName = imageWorker.Save("https://www.vsesmak.com.ua/sites/default/files/styles/large/public/field/image/syrnaya_gora_5330_1900_21.jpg?itok=RPUrRskl");
        var cheese = new CategoryEntity
        {
            Name = "Сири",
            Image = imageName,
            Description = "Cир – один з найчастіших гостей на нашому столі. " +
            "Адже це і смачно, і корисно, і доступно. Не можна сказати, що увесь, " +
            "що продається на прилавках супермаркетів твердий сир – неякісний."
        };

        imageName = imageWorker.Save("https://upload.wikimedia.org/wikipedia/commons/thumb/7/7b/Assorted_bread.jpg/420px-Assorted_bread.jpg");
        var bread = new CategoryEntity
        {
            Name = "Хліб",
            Image = imageName,
            Description = "У сегменті ринку «здорового харчування» існують сорти хліба, " +
            "які майже не сприяють набору зайвої ваги – наприклад, цільнозерновий хліб."
        };

        context.Categories.Add(kovbasa);
        context.Categories.Add(cheese);
        context.Categories.Add(bread);
        context.SaveChanges();
    }
}

app.Run();
