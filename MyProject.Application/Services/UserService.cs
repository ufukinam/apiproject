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
    public class UserService : BaseService
    {
        public UserService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork,mapper)
        {
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            var result = await _unitOfWork.GetRepository<User>().GetAllAsync();
            var usersDto = _mapper.Map<IEnumerable<UserDto>>(result);
            return usersDto;
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _unitOfWork.GetRepository<User>().GetByIdAsync(id);
        }

        public async Task<IEnumerable<User>> GetUsersByRoleIdAsync(int roleId)
        { //değişecek
            Expression<Func<User, bool>> filter = u => u.UserRoles.Any(ur => ur.RoleId == roleId);
            var userRepository = _unitOfWork.GetRepository<User>();
            return await userRepository.FindAsync(filter);
        }

        public async Task AddUserAsync(User user)
        {
            var userRepository = _unitOfWork.GetRepository<User>();
            await userRepository.AddAsync(user);
            await _unitOfWork.CompleteAsync();
        }

        public async Task UpdateUserAsync(User user)
        {
            var userRepository = _unitOfWork.GetRepository<User>();
            await userRepository.UpdateAsync(user);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteUserAsync(int id)
        {
            var userRepository = _unitOfWork.GetRepository<User>();
            var user = userRepository.GetByIdAsync(id);
            if (user != null)
            {
                await userRepository.DeleteAsync(id);
                await _unitOfWork.CompleteAsync();
            }
        }
    }
}