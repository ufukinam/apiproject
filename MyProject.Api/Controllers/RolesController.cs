using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyProject.Application.DTOs;
using MyProject.Application.Services;
using MyProject.Core.Entities;

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
            var roles = await _roleService.GetAllRolesAsync();
            return Ok(roles);
        }

        [HttpGet("VisibleRoles")]
        public async Task<IActionResult> GetVisibleRoles()
        {
            var roles = await _roleService.GetAllRolesAsync();
            return Ok(roles);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var role = await _roleService.GetRoleByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }
            return Ok(role);
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

        [HttpPost]
        public async Task<IActionResult> Create(RoleDto role)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _roleService.AddRoleAsync(role);
            return CreatedAtAction(nameof(GetById), new { id = role.Id }, role);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, RoleDto role)
        {
            if (id != role.Id)
            {
                return BadRequest();
            }
            await _roleService.UpdateRoleAsync(role);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _roleService.DeleteRoleAsync(id);
            return NoContent();
        }
    }
}