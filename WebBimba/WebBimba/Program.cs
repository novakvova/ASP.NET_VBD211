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
    context.Database.Migrate(); //������������ ������ ������� �� ��, ���� �� ��� ����

    if (!context.Categories.Any())
    {
        var imageName = imageWorker.Save("https://rivnepost.rv.ua/img/650/korisnoi-kovbasi-ne-buvae-hastroenterolohi-nazvali_20240612_4163.jpg");
        var kovbasa = new CategoryEntity
        {
            Name = "�������",
            Image = imageName,
            Description = "��� ����� ����������� �� ������� ������� �� ����������. " +
            "������� ��������, �� �� ��������, ���� ����� ������� �� ����� 50 ����� �� ����."
        };

        imageName = imageWorker.Save("https://www.vsesmak.com.ua/sites/default/files/styles/large/public/field/image/syrnaya_gora_5330_1900_21.jpg?itok=RPUrRskl");
        var cheese = new CategoryEntity
        {
            Name = "����",
            Image = imageName,
            Description = "C�� � ���� � ���������� ������ �� ������ ����. " +
            "���� �� � ������, � �������, � ��������. �� ����� �������, �� �����, " +
            "�� ��������� �� ��������� ������������ ������� ��� � ��������."
        };

        imageName = imageWorker.Save("https://upload.wikimedia.org/wikipedia/commons/thumb/7/7b/Assorted_bread.jpg/420px-Assorted_bread.jpg");
        var bread = new CategoryEntity
        {
            Name = "���",
            Image = imageName,
            Description = "� ������� ����� ���������� ����������� ������� ����� ����, " +
            "�� ����� �� �������� ������ ����� ���� � ���������, �������������� ���."
        };

        context.Categories.Add(kovbasa);
        context.Categories.Add(cheese);
        context.Categories.Add(bread);
        context.SaveChanges();
    }
}

app.Run();
