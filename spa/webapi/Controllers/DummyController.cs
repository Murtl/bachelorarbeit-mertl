using Microsoft.AspNetCore.Mvc;

namespace webapi.Controllers;

[ApiController]
[Route("[controller]")]
public class DummyController : ControllerBase
    {
    [HttpGet(Name = "DummyData")]
    public string Get()
    {
        return "Das ist ein Test!";
    }
}
