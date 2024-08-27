using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MyProject.Application.DTOs;
using MyProject.Application.Extensions;
using MyProject.Core.Entities;
using MyProject.Core.Interfaces;
using MyProject.Core.Models;

namespace MyProject.Application.Services
{
    public class PageService : BaseService
    {
        IRepository<Page> _pageRepository;
        public PageService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork,mapper)
        {
            _pageRepository = _unitOfWork.GetRepository<Page>();
        }

        public async Task<IEnumerable<PageDto>> GetAllPagesAsync()
        {
            Expression<Func<Page, bool>> filter = u => u.IsDeleted == false;
            Func<IQueryable<Page>, IOrderedQueryable<Page>> orderBy = q=> q.OrderBy(a=>a.Id);
            var result = await _pageRepository.GetAsync(filter: filter, orderBy: orderBy);
            var pagesDto = _mapper.Map<IEnumerable<PageDto>>(result);
            return pagesDto;
        }
        public async Task<PaginatedResult<PageDto>> GetPaginatedPagesAsync(int page, int pageSize, string sortBy, bool descending, string strFilter)
        {
            var query = _pageRepository.GetQuery();
            if (!string.IsNullOrEmpty(strFilter))
            {
                query = query.Where(u => u.IsDeleted == false 
                    && (EF.Functions.Like(u.Name, $"%{strFilter}%")));
            }
            else
            {
                query = query.Where(u => u.IsDeleted == false);
            }
            query = query.OrderByDynamic(sortBy, descending);
            var result = await _pageRepository.GetPaginatedAsync(query, page, pageSize);
            var pageDto = _mapper.Map<PaginatedResult<PageDto>>(result);
            return pageDto;
        }

        public async Task<Page> GetPageByIdAsync(int id)
        {
            return await _pageRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Page>> GetPagesByRoleIdAsync(int roleId)
        { //değişecek
            Expression<Func<Page, bool>> filter = r => r.RolePages.Any(ur => ur.RoleId == roleId);
            return await _pageRepository.GetAsync(filter: filter);
        }

        public async Task AddPageAsync(PageDto pageDto)
        {
            var page = _mapper.Map<Page>(pageDto);
            await _pageRepository.AddAsync(page);
            await _unitOfWork.CompleteAsync();
        }

        public async Task UpdatePageAsync(PageDto pageDto)
        {
            var page = _mapper.Map<Page>(pageDto);
            await _pageRepository.UpdateAsync(page);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeletePageAsync(int id)
        {
            var page = await _pageRepository.GetByIdAsync(id);
            if (page != null)
            {
                await _pageRepository.DeleteAsync(id);
                await _unitOfWork.CompleteAsync();
            }
        }
    }
}