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
    public class RolesController : ControllerBase
    {
        private readonly RoleService _roleService;

        public RolesController(RoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _roleService.GetAllRolesAsync();
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("VisibleRoles")]
        public async Task<IActionResult> GetVisibleRoles()
        {
            var result = await _roleService.GetAllRolesAsync();
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _roleService.GetRoleByIdAsync(id);
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("{id}/roles")]
        public async Task<IActionResult> GetRolesByUserId(int id)
        {
            var role = await _roleService.GetRolesByUserIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }
            return Ok(role);
        }
        
        [HttpGet("paginated")]
        public async Task<IActionResult> GetPaginated([FromQuery] PaginationInputModel paginationInputModel)
        {
            var result = await _roleService.GetPaginatedRolesAsync(paginationInputModel.Page, paginationInputModel.RowsPerPage, paginationInputModel.SortBy ?? string.Empty, paginationInputModel.Descending, paginationInputModel.StrFilter ?? string.Empty);
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }


        [HttpPost]
        public async Task<IActionResult> Create(RoleDto role)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
    
            var result = await _roleService.AddRoleAsync(role);
            if (!result.Success)
                return BadRequest(result);

            return CreatedAtAction(nameof(GetById), new { id = role.Id }, role);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, RoleDto role)
        {
            if (id != role.Id)
            {
                return BadRequest(ServiceResult<object>.FailureResult("ID mismatch"));
            }
            var result = await _roleService.UpdateRoleAsync(role);
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _roleService.DeleteRoleAsync(id);
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }
    }
}