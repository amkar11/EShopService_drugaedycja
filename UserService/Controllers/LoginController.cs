using Microsoft.AspNetCore.Mvc;

namespace UserService.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
