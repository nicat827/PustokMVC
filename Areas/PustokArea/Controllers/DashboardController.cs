using Microsoft.AspNetCore.Mvc;

namespace PustokApp.Areas.PustokArea.Controllers
{
    [Area("PustokArea")]

    public class DashboardController : Controller
    {
        public IActionResult Index()
        {

            return View();
        }
    }
}
