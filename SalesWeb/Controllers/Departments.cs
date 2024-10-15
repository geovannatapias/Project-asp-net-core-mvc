using Microsoft.AspNetCore.Mvc;

namespace SalesWeb.Controllers
{
    public class Departments : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
