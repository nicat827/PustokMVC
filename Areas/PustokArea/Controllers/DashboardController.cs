using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace PustokApp.Areas.PustokArea.Controllers
{
    [Area("PustokArea")]
    [Authorize(Roles = "Admin")]
    [AutoValidateAntiforgeryToken]
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {

            return View();
        }
    }
}
