using Microsoft.AspNetCore.Mvc;

namespace byook.Controllers
{
    public class MemberController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }

        public IActionResult ConsumerJoin()
        {
            return View();
        }

        public IActionResult SellerJoin()
        {
            return View();
        }

        public IActionResult SelectJoin()
        {
            return View();
        }
    }
}
