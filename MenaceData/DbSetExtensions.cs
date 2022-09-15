using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace MenaceData
{
    public static class DbSetExtensions
    {
        // Thank you Stack Overflow
        public static bool AddIfNotExists<T>(this DbSet<T> dbSet, T entity, Expression<Func<T, bool>> predicate = null) where T : class, new()
        {
            var exists = predicate == null ? dbSet.Any() : dbSet.Any(predicate);

            if (!exists) dbSet.Add(entity);

            return !exists;
        }
    }
}
