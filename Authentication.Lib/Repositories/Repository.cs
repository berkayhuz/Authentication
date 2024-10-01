using Authentication.Lib.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Authentication.Lib.Repositories
{
    public class Repository<T> : IRepository<T> where T : class, new()
    {
        private readonly AuthenticationDbContext _dbContext;

        public Repository(AuthenticationDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        private DbSet<T> Table => _dbContext.Set<T>();

        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));
            return await Table.FirstOrDefaultAsync(predicate).ConfigureAwait(false);
        }
    }
}
