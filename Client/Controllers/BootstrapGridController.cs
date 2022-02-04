using Microsoft.AspNetCore.Mvc;

namespace Client.Controllers
{
    public class BootstrapGridController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Task()
        {
            return View();
        }

        public IActionResult StarWars()
        {
            return View();
        }
    }
}
