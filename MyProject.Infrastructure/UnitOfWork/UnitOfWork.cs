using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyProject.Core.Entities;
using MyProject.Core.Interfaces;
using MyProject.Infrastructure.Data;
using MyProject.Infrastructure.Repositories;

namespace MyProject.Infrastructure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private readonly ConcurrentDictionary<string, object> _repositories;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            _repositories = new ConcurrentDictionary<string, object>();
        }

        public IUserRepository GetUserRepository()
        {
            if (!_repositories.ContainsKey(nameof(User)))
            {
                var repositoryInstance = new UserRepository(_context);
                _repositories[nameof(User)] = repositoryInstance;
            }

            return (IUserRepository)_repositories[nameof(User)];
        }

        public IRepository<T> GetRepository<T>() where T : class
        {
            var type = typeof(T).Name;

            if (!_repositories.ContainsKey(type))
            {
                var repositoryInstance = new Repository<T>(_context);
                _repositories[type] = repositoryInstance;
            }

            return (IRepository<T>)_repositories[type];
        }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task ExecuteInTransactionAsync(Func<Task> action)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    await action();
                    await transaction.CommitAsync();
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }
    }
}