using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyProject.Core.Entities;
using MyProject.Core.Interfaces;
using MyProject.Infrastructure.Data;

namespace MyProject.Infrastructure.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly AppDbContext _context;
        private readonly DbSet<User> _dbSet;
        public UserRepository(AppDbContext context) : base(context)
        {
            _context = context;
            _dbSet = context.Set<User>();
        }
        public async Task<IEnumerable<User>> GetUserWithRolesAsync(int userId)
        {
            return await _dbSet.Where(r => r.UserRoles.Any(ur => ur.UserId == userId)).ToListAsync();
        }
    }
}