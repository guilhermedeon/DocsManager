using DocsManager.Core.Application;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DocsManager.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(JwtService jwtService) : ControllerBase
    {
        [HttpGet("token")]
        [AllowAnonymous] // Allow access without authentication
        public IActionResult GetToken()
        {
            var token = jwtService.GenerateJwtToken();
            return Ok(new { Token = token });
        }
    }
}
