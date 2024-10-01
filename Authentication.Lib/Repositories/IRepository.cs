using System.Linq.Expressions;

namespace Authentication.Lib.Repositories
{
    public interface IRepository<T> where T : class, new()
    {
        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);

    }
}
