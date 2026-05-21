using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ITRockChallenge.Controllers
{
    [ApiController]
    [Route("test")]
    public class TestController : ControllerBase
    {
        [HttpGet("public")]
        public IActionResult Public()
        {
            return Ok("Public Endpoint");
        }

        [Authorize]
        [HttpGet("private")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult Private()
        {
            return Ok("Private endpoint");
        }
    }
}
