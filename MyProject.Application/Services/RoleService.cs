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
    public class RoleService : BaseService
    {
        IRepository<Role> _roleRepository;
        public RoleService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork,mapper)
        {
            _roleRepository = _unitOfWork.GetRepository<Role>();
        }

        public async Task<IEnumerable<RoleDto>> GetAllRolesAsync()
        {
            Expression<Func<Role, bool>> filter = u => u.IsDeleted == false;
            Func<IQueryable<Role>, IOrderedQueryable<Role>> orderBy = q=> q.OrderBy(a=>a.Id);
            var result = await _roleRepository.GetAsync(filter: filter, orderBy: orderBy);
            var rolesDto = _mapper.Map<IEnumerable<RoleDto>>(result);
            return rolesDto;
        }
        public async Task<PaginatedResult<RoleDto>> GetPaginatedRolesAsync(int page, int pageSize, string sortBy, bool descending, string strFilter)
        {
            var query = _roleRepository.GetQuery();
            if (!string.IsNullOrEmpty(strFilter))
            {
                query = query.Where(u => u.IsDeleted == false 
                    && (EF.Functions.Like(u.Name, $"%{strFilter}%")));
            }
            else
            {
                query = query.Where(u => u.IsDeleted == false);
            }
            query = query.Include(u => u.UserRoles);
            query = query.OrderByDynamic(sortBy, descending);
            var result = await _roleRepository.GetPaginatedAsync(query, page, pageSize);
            var roleDto = _mapper.Map<PaginatedResult<RoleDto>>(result);
            return roleDto;
        }

        public async Task<Role> GetRoleByIdAsync(int id)
        {
            return await _roleRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Role>> GetRolesByUserIdAsync(int userId)
        { //değişecek
            Expression<Func<Role, bool>> filter = u => u.UserRoles.Any(ur => ur.RoleId == userId);
            return await _roleRepository.GetAsync(filter: filter);
        }

        public async Task AddRoleAsync(RoleDto roleDto)
        {
            var role = _mapper.Map<Role>(roleDto);
            await _roleRepository.AddAsync(role);
            await _unitOfWork.CompleteAsync();
        }

        public async Task UpdateRoleAsync(RoleDto roleDto)
        {
            var role = _mapper.Map<Role>(roleDto);
            await _roleRepository.UpdateAsync(role);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteRoleAsync(int id)
        {
            var role = await _roleRepository.GetByIdAsync(id);
            
            if (role != null)
            {
                await _roleRepository.DeleteAsync(id);
                await _unitOfWork.CompleteAsync();
            }
        }
    }
}