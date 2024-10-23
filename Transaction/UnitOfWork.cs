using System.Data;

namespace WebAPITransaction.Transaction
{
    public class UnitOfWork :
        IUnitOfWork
    {
        private Dictionary<Guid, Tuple<IDbConnection, IDbTransaction>>? Dictionary;

        private readonly IDbConnection _connectionFactory;

        public UnitOfWork(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory.CreateConnectionAsync().GetAwaiter().GetResult();
        }

        public async Task BeginTransactionAsync(Guid key) => await Task.Run(() =>
        {
            Dictionary = new()
            {
                {
                    key,
                    new (_connectionFactory, _connectionFactory.BeginTransaction())
                }
            };
        });

        public async Task CommitTransactionAsync(Guid key) => await Task.Run(() =>
        {
            GetTransaction<IDbTransaction>(key)?.Commit();

            Dictionary?.Remove(key);
        });

        public async Task RollbackTransactionAsync(Guid key) => await Task.Run(() =>
        {
            GetTransaction<IDbTransaction>(key).Rollback();

            Dictionary?.Remove(key);
        });

        public async Task<T> GetInstanceAsync<T>(Guid key) => await Task.Run(() =>
        {
            if (Dictionary == null)
                Dictionary = new();

            return (T)Dictionary[key].Item1;
        });

        T GetTransaction<T>(Guid key) => Task.Run(() =>
        {
            if (Dictionary == null)
                Dictionary = new();

            return (T)Dictionary[key].Item2;

        }).GetAwaiter().GetResult();
    }
}
