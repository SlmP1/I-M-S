using Microsoft.AspNetCore.Mvc;

namespace IMS.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
