using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using User.Application;
using User.Domain.Models;

namespace UserService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : Controller
    {
        
        public readonly ILoginService _loginService;
        public LoginController(ILoginService loginService) {
            _loginService = loginService;
        }
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest loginRequest)
        {
            var token = _loginService.Login(loginRequest.Username, loginRequest.Password);
            return Ok(new { token });
        }

        [HttpGet]
        [Authorize(Policy = "AdminOnly")]
        public IActionResult AdminPage()
        {
            return Ok("Dane tylko dla administratora");
        }
    }
}
