using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace mpa.Controllers
{
    [Authorize(Roles = "Task.Apply")]
    public class ParticipantController : Controller
    {
        public IActionResult ApplyEvents()
        {
            return View();
        }

        public IActionResult CheckAppliedEvents()
        {
            return View();
        }
    }
}
