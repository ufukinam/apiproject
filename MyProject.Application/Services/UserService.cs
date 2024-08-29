using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MyProject.Application.DTOs;
using MyProject.Application.Extensions;
using MyProject.Application.Utils;
using MyProject.Core.Entities;
using MyProject.Core.Interfaces;
using MyProject.Core.Models;

namespace MyProject.Application.Services
{
    public class UserService : BaseService
    {
        IRepository<User> _userRepository;
        public UserService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork,mapper)
        {
            _userRepository = _unitOfWork.GetRepository<User>();
        }

        public async Task<ServiceResult<IEnumerable<UserDto>>> GetAllUsersAsync()
        {
            Expression<Func<User, bool>> filter = u => u.IsDeleted == false;
            Func<IQueryable<User>, IOrderedQueryable<User>> orderBy = q=> q.OrderBy(a=>a.Id);
            var result = await _userRepository.GetAsync(filter: filter, orderBy: orderBy);
            var usersDto = _mapper.Map<IEnumerable<UserDto>>(result);
            return ServiceResult<IEnumerable<UserDto>>.SuccessResult(usersDto);
        }

        public async Task<ServiceResult<PaginatedResult<UserDto>>> GetPaginatedUsersAsync(int page, int pageSize, string sortBy, bool descending, string strFilter)
        {
            var query = _userRepository.GetQuery();
            if (!string.IsNullOrEmpty(strFilter))
            {
                query = query.Where(u => u.IsDeleted == false 
                    && (EF.Functions.Like(u.Name, $"%{strFilter}%")
                        || EF.Functions.Like(u.Surname, $"%{strFilter}%")
                        || EF.Functions.Like(u.Email, $"%{strFilter}%")));
            }
            else
            {
                query = query.Where(u => u.IsDeleted == false);
            }
            query = query.Include(u => u.UserRoles);
            query = query.OrderByDynamic(sortBy, descending);
            var result = await _userRepository.GetPaginatedAsync(query, page, pageSize);
            var usersDto = _mapper.Map<PaginatedResult<UserDto>>(result);
            return ServiceResult<PaginatedResult<UserDto>>.SuccessResult(usersDto);
        }

        public async Task<ServiceResult<UserDto>> GetUserByIdAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                return ServiceResult<UserDto>.FailureResult("User not found");
            }
            var userDto = _mapper.Map<UserDto>(user);
            return ServiceResult<UserDto>.SuccessResult(userDto);
        }

        public async Task<IEnumerable<User>> GetUsersByRoleIdAsync(int roleId)
        { //değişecek
            Expression<Func<User, bool>> filter = u => u.UserRoles.Any(ur => ur.RoleId == roleId);
            return await _userRepository.GetAsync(filter: filter);
        }

        public async Task<ServiceResult<UserDto>> AddUserAsync(UserRegisterDto userDto)
        {
            var existingUser = await _userRepository.GetAsync(u => u.Email == userDto.Email);
            if (existingUser.Any())
            {
                return ServiceResult<UserDto>.FailureResult("User with this email already exists");
            }

            var user = _mapper.Map<User>(userDto);
            user.Password = PasswordHasher.HashPassword(userDto.Password);

            await _userRepository.AddAsync(user);
            await _unitOfWork.CompleteAsync();

            var createdUserDto = _mapper.Map<UserDto>(user);
            return ServiceResult<UserDto>.SuccessResult(createdUserDto);
        }

        public async Task<ServiceResult<UserDto>> UpdateUserAsync(UserUpdateDto userUpdateDto)
        {
            var existingUser = await _userRepository.GetByIdAsync(userUpdateDto.Id);
            if (existingUser == null)
            {
                return ServiceResult<UserDto>.FailureResult("User not found");
            }

            if (string.IsNullOrEmpty(userUpdateDto.Password))
            {
                userUpdateDto.Password = existingUser.Password;
            }
            else
            {
                userUpdateDto.Password = PasswordHasher.HashPassword(userUpdateDto.Password);
            }

            var user = _mapper.Map<User>(userUpdateDto);
            await _userRepository.UpdateAsync(user);
            await _unitOfWork.CompleteAsync();

            var updatedUserDto = _mapper.Map<UserDto>(user);
            return ServiceResult<UserDto>.SuccessResult(updatedUserDto);
        }

        public async Task<ServiceResult<bool>> DeleteUserAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                return ServiceResult<bool>.FailureResult("User not found");
            }

            await _userRepository.DeleteAsync(id);
            await _unitOfWork.CompleteAsync();

            return ServiceResult<bool>.SuccessResult(true);
        }

        public async Task<ServiceResult<UserDto>> RegisterAsync(UserRegisterDto registerDto)
        {
            //get user by email
            Expression<Func<User, bool>> filter = u => u.Email == registerDto.Email;
            var users = await _userRepository.GetAsync(filter: filter);

            //check if user exists
            if (users.Any()){
                return ServiceResult<UserDto>.FailureResult("User already exists");
            }

            //map dto to user
            var user = _mapper.Map<User>(registerDto);

            //hash password
            user.Password = PasswordHasher.HashPassword(registerDto.Password);

            await _userRepository.AddAsync(user);
            await _unitOfWork.CompleteAsync();

            return ServiceResult<UserDto>.SuccessResult(_mapper.Map<UserDto>(user));
        }

        public async Task<ServiceResult<UserDto>> AuthenticateAsync(string email, string password)
        {
            //get user by email
            Expression<Func<User, bool>> filter = u => u.Email == email && u.IsDeleted == false;
            var users = await _userRepository.GetAsync(filter: filter, includeProperties: "UserRoles");
            
            //check if user exists
            if (!users.Any())
            {
                return ServiceResult<UserDto>.FailureResult("Authentication failed");
            }
            //check if password is correct
            var user = users.Single();
            if(PasswordHasher.VerifyPassword(password, user!.Password))
            {
                //if password is correct
                return ServiceResult<UserDto>.SuccessResult(_mapper.Map<UserDto>(user));
            }
            //if password is not correct
            return ServiceResult<UserDto>.FailureResult("Authentication failed");
        }
    }
}