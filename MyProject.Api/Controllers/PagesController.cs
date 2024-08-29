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
            var result = await _pageService.GetAllPagesAsync();
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("paginated")]
        public async Task<IActionResult> GetPaginated([FromQuery] PaginationInputModel paginationInputModel)
        {
            var result = await _pageService.GetPaginatedPagesAsync(paginationInputModel.Page, paginationInputModel.RowsPerPage, paginationInputModel.SortBy ?? string.Empty, paginationInputModel.Descending, paginationInputModel.StrFilter ?? string.Empty);
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _pageService.GetPageByIdAsync(id);
            if (!result.Success)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpGet("{id}/pages")]
        public async Task<IActionResult> GetPagesByPageId(int id)
        {
            var result = await _pageService.GetPagesByRoleIdAsync(id);
            if (!result.Success)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(PageDto page)
        {
            var result = await _pageService.AddPageAsync(page);
            if (!result.Success)
                return BadRequest(result);

            return CreatedAtAction(nameof(GetById), new { id = page.Id }, page);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, PageDto page)
        {
            if (id != page.Id)
            {
                return BadRequest(ServiceResult<object>.FailureResult("ID mismatch"));
            }
            var result = await _pageService.UpdatePageAsync(page);
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _pageService.DeletePageAsync(id);
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }
    }
}