using Microsoft.AspNetCore.Mvc;
using MyProject.Application;
using MyProject.Application.DTOs;
using MyProject.Application.Services;

namespace MyProject.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly JwtHelper _jwtHelper;

        public AuthController(UserService userService, JwtHelper jwtHelper)
        {
            _userService = userService;
            _jwtHelper = jwtHelper;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userDto = await _userService.AuthenticateAsync(model.Email, model.Password);

            if (userDto == null)
                return Unauthorized();

            var token = _jwtHelper.GenerateJwtToken(userDto.Id.ToString(), userDto.Email);

            return Ok(new { Token = token, User = userDto });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userDto = await _userService.RegisterAsync(model);

            var token = _jwtHelper.GenerateJwtToken(userDto.Id.ToString(), userDto.Email);

            return Ok(new { Token = token, User = userDto });
        }
    }
}