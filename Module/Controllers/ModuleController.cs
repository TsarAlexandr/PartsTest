using Microsoft.AspNetCore.Mvc;

namespace Module.Controllers
{
    public class ModuleController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
