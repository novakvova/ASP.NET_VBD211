using Microsoft.AspNetCore.Mvc;
using WebBimba.Data;
using WebBimba.Models.Category;

namespace WebBimba.Controllers
{
    public class MainController : Controller
    {
        private readonly AppBimbaDbContext _dbContext;
        //Зберігає різну інформацію про MVC проект
        private readonly IWebHostEnvironment _environment;
        //DI - Depencecy Injection
        public MainController(AppBimbaDbContext context,
            IWebHostEnvironment environment)
        {
            _dbContext = context;
            _environment = environment;
        }

        //метод у контролері називаться - action - дія
        public IActionResult Index()
        {
            var model = _dbContext.Categories.ToList();
            return View(model);
        }

        [HttpGet] //це означає, що буде відображатися сторінки для перегляду
        public IActionResult Create()
        {
            //Ми повертає View - пусту, яка відобраєате сторінку де потрібно ввести дані для категорії
            return View();
        }

        [HttpPost] //це означає, що ми отримуємо дані із форми від клієнта
        public IActionResult Create(CategoryCreateViewModel model)
        {
            
            //Збережння в Базу даних інформації
            var dirName = "uploading";
            var dirSave = Path.Combine(_environment.WebRootPath, dirName);
            if (!Directory.Exists(dirSave))
            {
                Directory.CreateDirectory(dirSave);
            }
            if(model.Photo!=null)
            {
                //унікальне значенн, яке ніколи не повториться
                string fileName = Guid.NewGuid().ToString();
                var ext = Path.GetExtension(model.Photo.FileName);
                fileName += ext;
                var saveFile = Path.Combine(dirSave, fileName);
                using (var stream = new FileStream(saveFile, FileMode.Create)) 
                    model.Photo.CopyTo(stream);
            }
            //Те що ми отримали те і повертаємо назад
            return View(model);
        }
    }
}
