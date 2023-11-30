using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace mpa.Controllers
{
    [Authorize(Roles = "Task.Create")]
    public class OrganiserController : Controller
    {
        public IActionResult NewEvents()
        {
            return View();
        }

        public IActionResult CheckEvents()
        {
            return View();
        }
    }
}
