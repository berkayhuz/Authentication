using Authentication.Lib.Repositories;

namespace Authentication.Lib.UnitOfWorks
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        IRepository<T> GetRepository<T>() where T : class, new();

        Task<int> SaveAsync();
        int Save();
    }
}
