using System.Linq.Expressions;
using AutoMapper;
using MyProject.Application.DTOs;
using MyProject.Application.Utils;
using MyProject.Core.Entities;
using MyProject.Core.Interfaces;

namespace MyProject.Application.Services
{
    public class UserService : BaseService
    {
        IRepository<User> _userRepository;
        public UserService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork,mapper)
        {
            _userRepository = _unitOfWork.GetRepository<User>();
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            Expression<Func<User, bool>> filter = u => u.IsDeleted == false;
            Func<IQueryable<User>, IOrderedQueryable<User>> orderBy = q=> q.OrderBy(a=>a.Id);
            var result = await _userRepository.GetAsync(filter: filter, orderBy: orderBy);
            var usersDto = _mapper.Map<IEnumerable<UserDto>>(result);
            return usersDto;
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _userRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<User>> GetUsersByRoleIdAsync(int roleId)
        { //değişecek
            Expression<Func<User, bool>> filter = u => u.UserRoles.Any(ur => ur.RoleId == roleId);
            return await _userRepository.GetAsync(filter: filter);
        }

        public async Task AddUserAsync(User user)
        {
            await _userRepository.AddAsync(user);
            await _unitOfWork.CompleteAsync();
        }

        public async Task UpdateUserAsync(UserUpdateDto userUp)
        {
            if(userUp.Password==null || userUp.Password==""){
                var resUser = await _userRepository.GetByIdAsync(userUp.Id);
                userUp.Password = resUser.Password;
            }else{
                userUp.Password = PasswordHasher.HashPassword(userUp.Password);
            }
            var user = _mapper.Map<User>(userUp);

            await _userRepository.UpdateAsync(user);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteUserAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user != null)
            {
                await _userRepository.DeleteAsync(id);
                await _unitOfWork.CompleteAsync();
            }
        }

        public async Task<UserDto> RegisterAsync(UserRegisterDto registerDto)
        {
            var user = _mapper.Map<User>(registerDto);

            user.Password = PasswordHasher.HashPassword(registerDto.Password);

            await _userRepository.AddAsync(user);
            await _unitOfWork.CompleteAsync();

            return _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto> AuthenticateAsync(string email, string password)
        {
            Expression<Func<User, bool>> filter = u => u.Email == email;
            var users = await _userRepository.GetAsync(filter: filter, includeProperties: "UserRoles");
            
            // If you have hashed passwords, you should compare the hashed values here
            // if (user != null && VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))

             // Verify the hashed password
            if (users != null && users.Count()!=0)
            {
                var user = users.FirstOrDefault();
                if(PasswordHasher.VerifyPassword(password, user!.Password))
                {
                    return _mapper.Map<UserDto>(user);
                }
            }

            return null;
        }
    }
}