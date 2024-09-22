using Microsoft.AspNetCore.Mvc;
using WebBimba.Data;
using WebBimba.Models.Category;

namespace WebBimba.Controllers
{
    public class MainController : Controller
    {
        private readonly AppBimbaDbContext _dbContext;

        //DI - Depencecy Injection
        public MainController(AppBimbaDbContext context)
        {
            _dbContext = context;
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

            //Те що ми отримали те і повертаємо назад
            return View(model);
        }
    }
}
