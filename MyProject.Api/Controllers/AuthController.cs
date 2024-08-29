using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyProject.Application;
using MyProject.Application.DTOs;
using MyProject.Application.Services;
using MyProject.Core.Models;

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
                return BadRequest(ServiceResult<object>.FailureResult("Invalid model state", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()));

            var result = await _userService.AuthenticateAsync(model.Email, model.Password);

            if (!result.Success)
                return Unauthorized(result);

            var token = _jwtHelper.GenerateJwtToken(result.Data.Id.ToString(), result.Data.Email);

            return Ok(ServiceResult<object>.SuccessResult(new { Token = token, User = result.Data }));
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ServiceResult<object>.FailureResult("Invalid model state", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()));

            var result = await _userService.RegisterAsync(model);

            if (!result.Success)
                return BadRequest(result);
    
            var token = _jwtHelper.GenerateJwtToken(result.Data.Id.ToString(), result.Data.Email);

            return Ok(ServiceResult<object>.SuccessResult(new { Token = token, User = result.Data }));
        }
    }
}