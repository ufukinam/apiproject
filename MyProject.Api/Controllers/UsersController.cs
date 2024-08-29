using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyProject.Application.DTOs;
using MyProject.Application.Services;
using MyProject.Core.Entities;
using MyProject.Core.Models;

namespace MyProject.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly UserService _userService;

        public UsersController(UserService userService)
        {
            _userService = userService;
        }
        
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _userService.GetAllUsersAsync();
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("paginated")]
        public async Task<IActionResult> GetPaginated([FromQuery] PaginationInputModel paginationInputModel)
        {
            var result = await _userService.GetPaginatedUsersAsync(paginationInputModel.Page, paginationInputModel.RowsPerPage, paginationInputModel.SortBy ?? string.Empty, paginationInputModel.Descending, paginationInputModel.StrFilter ?? string.Empty);
           if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _userService.GetUserByIdAsync(id);
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("{id}/roles")]
        public async Task<IActionResult> GetUserByRoleId(int id)
        {
            var user = await _userService.GetUsersByRoleIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserRegisterDto user)
        {
            if (!ModelState.IsValid)
                return BadRequest(ServiceResult<object>.FailureResult("Invalid model state", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()));

            var result = await _userService.RegisterAsync(user);
            if (!result.Success)
                return BadRequest(result);

            return CreatedAtAction(nameof(GetById), new { id = result.Data.Id }, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UserUpdateDto user)
        {
            if (!ModelState.IsValid)
                return BadRequest(ServiceResult<object>.FailureResult("Invalid model state", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()));
        
            if (id != user.Id) 
                return BadRequest(ServiceResult<object>.FailureResult("ID mismatch"));

            var result = await _userService.UpdateUserAsync(user);
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _userService.DeleteUserAsync(id);
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }
        
    }
}