using System.Data;

namespace WebAPITransaction.Transaction
{
    public class ConnectionFactory(IDbConnection dbConnection) : IConnectionFactory
    {
        async Task<IDbConnection> IConnectionFactory.CreateConnectionAsync()
        {
            return await Task.Factory.StartNew(() =>
            {
                var connection = (IDbConnection)((ICloneable)dbConnection).Clone();

                if (connection.State == ConnectionState.Closed)
                    connection.Open();

                return connection;

            });
        }
    }
}
