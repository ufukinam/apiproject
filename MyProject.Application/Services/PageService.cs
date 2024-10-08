using System.Linq.Expressions;
using AutoMapper;
using MyProject.Application.DTOs;
using MyProject.Core.Entities;
using MyProject.Core.Interfaces;

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