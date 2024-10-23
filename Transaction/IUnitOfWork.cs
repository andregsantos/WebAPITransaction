namespace WebAPITransaction.Transaction
{
    public interface IUnitOfWork
    {
        Task BeginTransactionAsync(Guid key);
        Task CommitTransactionAsync(Guid key);
        Task RollbackTransactionAsync(Guid key);
        Task<T> GetInstanceAsync<T>(Guid key);
    }
}
