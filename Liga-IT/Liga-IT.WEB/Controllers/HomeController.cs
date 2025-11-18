using Liga_IT.WEB.Filters;
using Liga_IT.WEB.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Liga_IT.WEB.Controllers
{
    [AuthorizeSession]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
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
