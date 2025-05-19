using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using User.Application;
using User.Domain.DTO;
using User.Domain.Models;
using System.Collections.Generic;
using System.Security.Claims;

namespace UserService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : Controller
    {
        
        public readonly ILoginService _loginService;
        public readonly IUserDataService _userDataService;
        public readonly IMapper _mapper;
        private readonly Queue<string> _loginMessages;
        public LoginController(ILoginService loginService,
            IUserDataService userDataService,
            IMapper mapper,
            Queue<string> loginMessages)
        {
            _loginService = loginService;
            _userDataService = userDataService;
            _mapper = mapper;
            _loginMessages = loginMessages;
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            var token = await _loginService.LoginAsync(loginRequest.Username, loginRequest.Password);
            string message = $"User {loginRequest.Username} succefully logged in";
            _loginMessages.Enqueue(message);
            Console.WriteLine(message);
            Console.WriteLine($"Elements in queue: {string.Join(", ", _loginMessages)}");
            return Ok(new { token });
        }

        [HttpGet("admin")]
        [Authorize(Policy = "AdminOnly")]
        public IActionResult AdminPage()
        {
            return Ok("Dane tylko dla administratora");
        }

        [HttpGet("user")]
        [Authorize]
        public async Task<IActionResult> GetUserData()
        {
            try
            {
                string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (userId == null) return BadRequest("User ID not found in token");
                UserDb? user = await _userDataService.GetUserDataAsync(Convert.ToInt32(userId));
                var user_dto = _mapper.Map<UserDto>(user);
                return Ok(user_dto);
                
            }
            catch (InvalidOperationException ex)
            {
                return NotFound($"{ex.Message}");
            }
            
        }
    }
}
