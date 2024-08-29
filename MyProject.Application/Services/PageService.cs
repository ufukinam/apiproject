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

        public async Task<ServiceResult<IEnumerable<PageDto>>> GetAllPagesAsync()
        {
            Expression<Func<Page, bool>> filter = u => u.IsDeleted == false;
            Func<IQueryable<Page>, IOrderedQueryable<Page>> orderBy = q=> q.OrderBy(a=>a.Id);
            var result = await _pageRepository.GetAsync(filter: filter, orderBy: orderBy);
            var pagesDto = _mapper.Map<IEnumerable<PageDto>>(result);
            return ServiceResult<IEnumerable<PageDto>>.SuccessResult(pagesDto);

        }
        public async Task<ServiceResult<PaginatedResult<PageDto>>> GetPaginatedPagesAsync(int page, int pageSize, string sortBy, bool descending, string strFilter)
        {
            var query = _pageRepository.GetQuery();
            if (!string.IsNullOrEmpty(strFilter))
            {
                query = query.Where(u => u.IsDeleted == false 
                    && (EF.Functions.Like(u.Name, $"%{strFilter}%") || EF.Functions.Like(u.Url, $"%{strFilter}%") || EF.Functions.Like(u.Label, $"%{strFilter}%") || EF.Functions.Like(u.Icon, $"%{strFilter}%")));
            }
            else
            {
                query = query.Where(u => u.IsDeleted == false);
            }
            query = query.OrderByDynamic(sortBy, descending);
            var result = await _pageRepository.GetPaginatedAsync(query, page, pageSize);
            var pageDto = _mapper.Map<PaginatedResult<PageDto>>(result);
            return ServiceResult<PaginatedResult<PageDto>>.SuccessResult(pageDto);
        }

        public async Task<ServiceResult<PageDto>> GetPageByIdAsync(int id)
        {
            var page = await _pageRepository.GetByIdAsync(id);
            if (page == null)
            {
                return ServiceResult<PageDto>.FailureResult("Page not found");
            }
            var pageDto = _mapper.Map<PageDto>(page);
            return ServiceResult<PageDto>.SuccessResult(pageDto);
        }

        public async Task<ServiceResult<IEnumerable<PageDto>>> GetPagesByRoleIdAsync(int roleId)
        { //değişecek
            Expression<Func<Page, bool>> filter = r => r.RolePages.Any(ur => ur.RoleId == roleId);
            var pages = await _pageRepository.GetAsync(filter: filter);
            var pagesDto = _mapper.Map<IEnumerable<PageDto>>(pages);
            return ServiceResult<IEnumerable<PageDto>>.SuccessResult(pagesDto);
        }

        public async Task<ServiceResult<PageDto>> AddPageAsync(PageDto pageDto)
        {
            var existingPage = await _pageRepository.GetAsync(u => u.Name == pageDto.Name);
            if (existingPage.Any())
            {
                return ServiceResult<PageDto>.FailureResult("Page with this name already exists");
            }
            var page = _mapper.Map<Page>(pageDto);
            await _pageRepository.AddAsync(page);
            await _unitOfWork.CompleteAsync();
            var createdPageDto = _mapper.Map<PageDto>(page);
            return ServiceResult<PageDto>.SuccessResult(createdPageDto);
        }

        public async Task<ServiceResult<PageDto>> UpdatePageAsync(PageDto pageDto)
        {
            var existingPage = await _pageRepository.GetByIdAsync(pageDto.Id);
            if (existingPage == null)
            {
                return ServiceResult<PageDto>.FailureResult("Page not found");
            }
            var page = _mapper.Map<Page>(pageDto);
            await _pageRepository.UpdateAsync(page);
            await _unitOfWork.CompleteAsync();
            var updatedPageDto = _mapper.Map<PageDto>(page);
            return ServiceResult<PageDto>.SuccessResult(updatedPageDto);
        }

        public async Task<ServiceResult<bool>> DeletePageAsync(int id)
        {
            var page = await _pageRepository.GetByIdAsync(id);
            if (page == null)
            {
                return ServiceResult<bool>.FailureResult("Page not found");
            }
            await _pageRepository.DeleteAsync(id);
            await _unitOfWork.CompleteAsync();
            return ServiceResult<bool>.SuccessResult(true);
        }
    }
}