using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Movies.Client.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
