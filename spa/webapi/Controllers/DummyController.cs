using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace webapi.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class DummyController : ControllerBase
    {
    [HttpGet(Name = "DummyData")]
    [Authorize(Roles = "Task.Create")]
    public string Get()
    {
        return "Das ist ein Test!";
    }
}
