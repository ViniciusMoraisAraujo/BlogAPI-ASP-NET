using BlogAs.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace BlogAs.Controllers;

[ApiController]
[Route("")]
public class HomeController : ControllerBase
{
    [HttpGet("")]
    [ApiKey]
    public IActionResult Get()
    {
        return Ok();
    }
}