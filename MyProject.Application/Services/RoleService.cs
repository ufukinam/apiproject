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

        public async Task<ServiceResult<IEnumerable<RoleDto>>> GetAllRolesAsync()
        {
            Expression<Func<Role, bool>> filter = u => u.IsDeleted == false;
            Func<IQueryable<Role>, IOrderedQueryable<Role>> orderBy = q=> q.OrderBy(a=>a.Id);
            var result = await _roleRepository.GetAsync(filter: filter, orderBy: orderBy);
            var rolesDto = _mapper.Map<IEnumerable<RoleDto>>(result);
            return ServiceResult<IEnumerable<RoleDto>>.SuccessResult(rolesDto);
        }
        public async Task<ServiceResult<PaginatedResult<RoleDto>>> GetPaginatedRolesAsync(int page, int pageSize, string sortBy, bool descending, string strFilter)
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
            return ServiceResult<PaginatedResult<RoleDto>>.SuccessResult(roleDto);
        }

        public async Task<ServiceResult<RoleDto>> GetRoleByIdAsync(int id)
        {
            var role = await _roleRepository.GetByIdAsync(id);
            if (role == null)
            {
                return ServiceResult<RoleDto>.FailureResult("Role not found");
            }
            var roleDto = _mapper.Map<RoleDto>(role);
            return ServiceResult<RoleDto>.SuccessResult(roleDto);
        }

        public async Task<IEnumerable<Role>> GetRolesByUserIdAsync(int userId)
        { //değişecek
            Expression<Func<Role, bool>> filter = u => u.UserRoles.Any(ur => ur.RoleId == userId);
            return await _roleRepository.GetAsync(filter: filter);
        }

        public async Task<ServiceResult<RoleDto>> AddRoleAsync(RoleDto roleDto)
        {
            var existingRole = await _roleRepository.GetAsync(u => u.Name == roleDto.Name);
            if (existingRole.Any())
            {
                return ServiceResult<RoleDto>.FailureResult("Role with this name already exists");
            }
            var role = _mapper.Map<Role>(roleDto);
            await _roleRepository.AddAsync(role);
            await _unitOfWork.CompleteAsync();
            var createdRoleDto = _mapper.Map<RoleDto>(role);
            return ServiceResult<RoleDto>.SuccessResult(createdRoleDto);
        }

        public async Task<ServiceResult<RoleDto>> UpdateRoleAsync(RoleDto roleDto)
        {
            var existingRole = await _roleRepository.GetByIdAsync(roleDto.Id);
            if (existingRole == null)
            {
                return ServiceResult<RoleDto>.FailureResult("Role not found");
            }
            var role = _mapper.Map<Role>(roleDto);
            await _roleRepository.UpdateAsync(role);
            await _unitOfWork.CompleteAsync();
            var updatedRoleDto = _mapper.Map<RoleDto>(role);
            return ServiceResult<RoleDto>.SuccessResult(updatedRoleDto);
        }

        public async Task<ServiceResult<bool>> DeleteRoleAsync(int id)
        {
            var role = await _roleRepository.GetByIdAsync(id);
            if (role == null)
            {
                return ServiceResult<bool>.FailureResult("Role not found");
            }
            await _roleRepository.DeleteAsync(id);
            await _unitOfWork.CompleteAsync();
            return ServiceResult<bool>.SuccessResult(true);
        }
    }
}