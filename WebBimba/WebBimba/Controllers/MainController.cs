using Microsoft.AspNetCore.Mvc;

namespace WebBimba.Controllers
{
    public class MainController : Controller
    {
        //метод у контролері називаться - action - дія
        public IActionResult Index()
        {
            //return "Моніка - привіт!";
            return View();
        }
    }
}
