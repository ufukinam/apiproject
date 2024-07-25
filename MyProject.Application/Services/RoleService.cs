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
    public class RoleService : BaseService
    {
        public RoleService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork,mapper)
        {
        }

        public async Task<IEnumerable<Role>> GetAllRolesAsync()
        {
            return await _unitOfWork.GetRepository<Role>().GetAllAsync();
        }

        public async Task<Role> GetRoleByIdAsync(int id)
        {
            return await _unitOfWork.GetRepository<Role>().GetByIdAsync(id);
        }

        public async Task<IEnumerable<Role>> GetRolesByUserIdAsync(int userId)
        { //değişecek
            Expression<Func<Role, bool>> filter = r => r.UserRoles.Any(ur => ur.UserId == userId);
            var roleRepository = _unitOfWork.GetRepository<Role>();
            return await roleRepository.FindAsync(filter);
        }

        public async Task AddRoleAsync(Role role)
        {
            var roleRepository = _unitOfWork.GetRepository<Role>();
            await roleRepository.AddAsync(role);
            await _unitOfWork.CompleteAsync();
        }

        public async Task UpdateRoleAsync(Role role)
        {
            var roleRepository = _unitOfWork.GetRepository<Role>();
            await roleRepository.UpdateAsync(role);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteRoleAsync(int id)
        {
            var roleRepository = _unitOfWork.GetRepository<Role>();
            var role = roleRepository.GetByIdAsync(id);
            if (role != null)
            {
                await roleRepository.DeleteAsync(id);
                await _unitOfWork.CompleteAsync();
            }
        }
    }
}