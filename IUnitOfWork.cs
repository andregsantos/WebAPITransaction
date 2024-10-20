using Microsoft.Data.SqlClient;
using System.Data;
using System.Data.Common;
using System.Transactions;

namespace WebAPITransaction
{
    public interface IUnitOfWork 
    {
        void BeginTransaction(Guid key);
        void CommitTransaction(Guid key);
        void RollbackTransaction(Guid key);
        SqlTransaction GetTransaction(Guid key);
        SqlConnection GetConnection(Guid key);
        void Dispose(Guid key);
    }
}
