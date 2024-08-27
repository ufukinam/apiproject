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
    public class PagesController : ControllerBase
    {
        private readonly PageService _pageService;

        public PagesController(PageService pageService)
        {
            _pageService = pageService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var pages = await _pageService.GetAllPagesAsync();
            return Ok(pages);
        }

        [HttpGet("paginated")]
        public async Task<IActionResult> GetPaginated([FromQuery] PaginationInputModel paginationInputModel)
        {
            var paginatedUsers = await _pageService.GetPaginatedPagesAsync(paginationInputModel.Page, paginationInputModel.RowsPerPage, paginationInputModel.SortBy, paginationInputModel.Descending, paginationInputModel.StrFilter);
            return Ok(paginatedUsers);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var page = await _pageService.GetPageByIdAsync(id);
            if (page == null)
            {
                return NotFound();
            }
            return Ok(page);
        }

        [HttpGet("{id}/pages")]
        public async Task<IActionResult> GetPagesByPageId(int id)
        {
            var page = await _pageService.GetPagesByRoleIdAsync(id);
            if (page == null)
            {
                return NotFound();
            }
            return Ok(page);
        }

        [HttpPost]
        public async Task<IActionResult> Create(PageDto page)
        {
            await _pageService.AddPageAsync(page);
            return CreatedAtAction(nameof(GetById), new { id = page.Id }, page);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, PageDto page)
        {
            if (id != page.Id)
            {
                return BadRequest();
            }
            await _pageService.UpdatePageAsync(page);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _pageService.DeletePageAsync(id);
            return NoContent();
        }
    }
}