using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using UB.WebUI.Models;

namespace UB.WebUI.Controllers
{
    public class HomeController : Controller
    {


        public IActionResult Index()
        {
            // Pass the required ViewData for cards
            ViewData["TotalInvoices"] = 22;
            ViewData["TotalPayments"] = 1223;

            return View();
        }

        [HttpGet]
        public JsonResult Dashboard()
        {
            // Example dataset (replace with database query)
            var graphData = new List<object>
        {
            new { Month = "January", Payment = 20000 },
            new { Month = "February", Payment = 0 },
            new { Month = "March", Payment = 0 },
            new { Month = "April", Payment = 0 },
            new { Month = "May", Payment = 0 }
        };

            return Json(graphData);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
