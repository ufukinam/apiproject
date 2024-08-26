using System.Linq.Expressions;
using MyProject.Core.Models;

namespace MyProject.Core.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = "");
        Task<T> GetByIdAsync(int id);
        IQueryable<T> GetQuery();
        Task<PaginatedResult<T>> GetPaginatedAsync(IQueryable<T> query, int pageNumber = 1, int pageSize = 10);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(int id);
    }
}