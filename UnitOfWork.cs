using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.Data.SqlClient;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.Common;

namespace WebAPITransaction
{

    public class UnitOfWork(SqlConnection initialConnection) : 
        IUnitOfWork
    {
        private Dictionary<Guid, Tuple<SqlConnection, SqlTransaction>> _dictionary = new();

        private SqlConnection CloneConnection(SqlConnection connection)
        {
            return (SqlConnection)((ICloneable)connection).Clone();
        }

        public void BeginTransaction(Guid key)
        {
            var connection = CloneConnection(initialConnection);

            connection.Open();

            _dictionary.Add(key, new Tuple<SqlConnection, SqlTransaction>(connection,
                    connection.BeginTransaction()));
        }

        public void CommitTransaction(Guid key)
        {
            GetTransaction(key)?.Commit();
        }           

        public void RollbackTransaction(Guid key)
        {
            GetTransaction(key).Rollback();
        }

        public SqlConnection GetConnection(Guid key)
        {
            if (_dictionary[key].Item1.State == ConnectionState.Closed)
                _dictionary[key].Item1.Open();

            return _dictionary[key].Item1;
        }

        public SqlConnection GetConnection()
        {
            return initialConnection;
        }

        public SqlTransaction GetTransaction(Guid key)
        {
           return _dictionary[key].Item2;
        }

        public void Dispose(Guid key)
        {
            _dictionary.Remove(key);
        }
    }
}
