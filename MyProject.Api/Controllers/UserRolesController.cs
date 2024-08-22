using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyProject.Application.DTOs;
using MyProject.Application.Services;

namespace MyProject.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserRolesController : ControllerBase
    {
        private readonly UserRoleService _userRoleService;

        public UserRolesController(UserRoleService userRolesService)
        {
            _userRoleService = userRolesService;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userRoleService.GetAllUserRolesAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var userRole = await _userRoleService.GetUserRoleByIdAsync(id);
            if (userRole == null)
            {
                return NotFound();
            }
            return Ok(userRole);
        }

        [HttpGet("{id}/roles")]
        public async Task<IActionResult> GetUserByRoleId(int id)
        {
            var userRole = await _userRoleService.GetUserRoleByIdAsync(id);
            if (userRole == null)
            {
                return NotFound();
            }
            return Ok(userRole);
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserRoleDto userRoleDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _userRoleService.AddUserRoleAsync(userRoleDto);

            return CreatedAtAction(nameof(GetById), new { id = userRoleDto.Id }, userRoleDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UserRoleDto userRoleDto)
        {
            if (id != userRoleDto.Id)
            {
                return BadRequest();
            }

            await _userRoleService.UpdateUserRoleAsync(userRoleDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _userRoleService.DeleteUserAsync(id);
            return NoContent();
        }
    }
}