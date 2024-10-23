using System.Data;

namespace WebAPITransaction.Transaction
{
    public interface IConnectionFactory
    {
        Task<IDbConnection> CreateConnectionAsync();
    }
}
