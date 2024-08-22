using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using MyProject.Application.DTOs;
using MyProject.Core.Entities;
using MyProject.Core.Interfaces;

namespace MyProject.Application.Services
{
    public class UserRoleService: BaseService
    {
        IRepository<UserRole> _userRoleRepository;
        public UserRoleService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork,mapper)
        {
            _userRoleRepository = _unitOfWork.GetRepository<UserRole>();
        }

        public async Task<IEnumerable<UserRoleDto>> GetAllUserRolesAsync()
        {
            Expression<Func<UserRole, bool>> filter = u => u.IsDeleted == false;
            Func<IQueryable<UserRole>, IOrderedQueryable<UserRole>> orderBy = q=> q.OrderBy(a=>a.Id);
            var result = await _userRoleRepository.GetAsync(filter: filter, orderBy: orderBy);
            var usersDto = _mapper.Map<IEnumerable<UserRoleDto>>(result);
            return usersDto;
        }

        public async Task<UserRole> GetUserRoleByIdAsync(int id)
        {
            return await _userRoleRepository.GetByIdAsync(id);
        }

        public async Task AddUserRoleAsync(UserRoleDto userRoleDto)
        {
            var userRole = _mapper.Map<UserRole>(userRoleDto);
            await _userRoleRepository.AddAsync(userRole);
            await _unitOfWork.CompleteAsync();
        }

        public async Task UpdateUserRoleAsync(UserRoleDto userRoleDto)
        {
            var userRole = _mapper.Map<UserRole>(userRoleDto);

            await _userRoleRepository.UpdateAsync(userRole);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteUserAsync(int id)
        {
            var userRole = await _userRoleRepository.GetByIdAsync(id);
            if (userRole != null)
            {
                await _userRoleRepository.DeleteAsync(id);
                await _unitOfWork.CompleteAsync();
            }
        }
    }
}