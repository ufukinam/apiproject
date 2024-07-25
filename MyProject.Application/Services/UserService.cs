using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyProject.Core.Entities;
using MyProject.Core.Interfaces;

namespace MyProject.Application.Services
{
    public class UserService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _unitOfWork.GetRepository<User>().GetAllAsync();
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _unitOfWork.GetRepository<User>().GetByIdAsync(id);
        }

        public async Task<User> GetUserWithRolesAsync(int id)
        { //değişecek
            return await _unitOfWork.GetRepository<User>().GetByIdAsync(id);;
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