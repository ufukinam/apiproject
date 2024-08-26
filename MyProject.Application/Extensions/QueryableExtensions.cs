using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Reflection;

namespace MyProject.Application.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> OrderByDynamic<T>(this IQueryable<T> query, string sortBy, bool descending)
        {
            var parameter = Expression.Parameter(typeof(T), "p");
            var property = Expression.Property(parameter, sortBy);
            var lambda = Expression.Lambda(property, parameter);

            string methodName = descending ? "OrderByDescending" : "OrderBy";
            var resultExp = Expression.Call(typeof(Queryable), methodName, new Type[] { typeof(T), property.Type }, query.Expression, Expression.Quote(lambda));

            return query.Provider.CreateQuery<T>(resultExp);
        }
    }
}