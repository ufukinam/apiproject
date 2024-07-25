using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using MyProject.Core.Entities;
using MyProject.Core.Interfaces;

namespace MyProject.Application.Services
{
    public class PageService : BaseService
    {
        public PageService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork,mapper)
        {
        }

        public async Task<IEnumerable<Page>> GetAllPagesAsync()
        {
            return await _unitOfWork.GetRepository<Page>().GetAllAsync();
        }

        public async Task<Page> GetPageByIdAsync(int id)
        {
            return await _unitOfWork.GetRepository<Page>().GetByIdAsync(id);
        }

        public async Task<IEnumerable<Page>> GetPagesByRoleIdAsync(int roleId)
        { //değişecek
            Expression<Func<Page, bool>> filter = r => r.RolePages.Any(ur => ur.RoleId == roleId);
            var pageRepository = _unitOfWork.GetRepository<Page>();
            return await pageRepository.FindAsync(filter);
        }

        public async Task AddPageAsync(Page page)
        {
            var pageRepository = _unitOfWork.GetRepository<Page>();
            await pageRepository.AddAsync(page);
            await _unitOfWork.CompleteAsync();
        }

        public async Task UpdatePageAsync(Page page)
        {
            var pageRepository = _unitOfWork.GetRepository<Page>();
            await pageRepository.UpdateAsync(page);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeletePageAsync(int id)
        {
            var pageRepository = _unitOfWork.GetRepository<Page>();
            var page = pageRepository.GetByIdAsync(id);
            if (page != null)
            {
                await pageRepository.DeleteAsync(id);
                await _unitOfWork.CompleteAsync();
            }
        }
    }
}