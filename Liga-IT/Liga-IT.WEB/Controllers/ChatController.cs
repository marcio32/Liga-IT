using Liga_IT.WEB.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Liga_IT.WEB.Controllers
{
    [AuthorizeSession]
    public class ChatController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
