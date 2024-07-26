using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyProject.Application.Services;
using MyProject.Core.Entities;

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
        public async Task<IActionResult> Create(Page page)
        {
            await _pageService.AddPageAsync(page);
            return CreatedAtAction(nameof(GetById), new { id = page.Id }, page);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Page page)
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