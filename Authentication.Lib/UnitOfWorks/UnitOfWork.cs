using Authentication.Lib.Data;
using Authentication.Lib.Repositories;

namespace Authentication.Lib.UnitOfWorks
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AuthenticationDbContext dbContext;

        public UnitOfWork(AuthenticationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async ValueTask DisposeAsync()
        {
            await dbContext.DisposeAsync();
        }

        public int Save()
        {
            return dbContext.SaveChanges();
        }

        public async Task<int> SaveAsync()
        {
            return await dbContext.SaveChangesAsync();
        }

        IRepository<T> IUnitOfWork.GetRepository<T>()
        {
            return new Repository<T>(dbContext);
        }
    }
}
