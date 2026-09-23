using Microsoft.AspNetCore.Mvc;

namespace LTWeb2_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Web API đang hoạt động");
        }
    }
}