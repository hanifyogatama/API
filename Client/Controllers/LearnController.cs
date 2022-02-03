using Microsoft.AspNetCore.Mvc;

namespace Client.Controllers
{
    public class LearnController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
