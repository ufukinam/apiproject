using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyProject.Core.Entities;
using MyProject.Core.Interfaces;
using MyProject.Infrastructure.Data;

namespace MyProject.Infrastructure.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(AppDbContext context) : base(context)
        {
        }
        public async Task<IEnumerable<User>> GetUserWithRolesAsync(int userId)
        {
            return await _dbSet.Where(r => r.UserRoles.Any(ur => ur.UserId == userId)).ToListAsync();
        }
    }
}