using Microsoft.AspNetCore.Mvc;
using WebBimba.Data;

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
    }
}
