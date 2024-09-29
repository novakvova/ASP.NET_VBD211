using Microsoft.AspNetCore.Mvc;
using WebBimba.Data;
using WebBimba.Data.Entities;
using WebBimba.Models.Category;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
            var entity = new CategoryEntity();
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
                entity.Image = fileName;
            }
            entity.Name = model.Name;
            entity.Description = model.Description;
            _dbContext.Categories.Add(entity);
            _dbContext.SaveChanges();
            //Переходимо до списку усіх категорій, тобото визиваємо метод Index нашого контролера
            return Redirect("/");
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var category = _dbContext.Categories.Find(id);
            if (category == null)
            {
                return NotFound();
            }

            if (!string.IsNullOrEmpty(category.Image))
            {
                var dirName = "uploading";
                var fileSave = Path.Combine(_environment.WebRootPath, dirName, category.Image);
                if (System.IO.File.Exists(fileSave)) 
                    System.IO.File.Delete(fileSave);
            }
            
            _dbContext.Categories.Remove(category);
            _dbContext.SaveChanges();

            return Json(new { text="Ми його видалили" }); // Вертаю об'єкт у відповідь
        }
    }
}
