using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Net.Http;
using System.Text;

namespace WebAPITransaction.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class OrderController(IUnitOfWork _unitOfWork) : ControllerBase
    {
        [HttpPost("InsertOrderHeader")]
        public int InsertOrderHeader()
        {
            return Execute("INSERT INTO OrderHeader (OrderID ,CustomerName ,OrderDate) VALUES (1 ,'CustomerName teste' ,'2024-01-01')");
        }

        [HttpPost("InsertOrderDetails")]
        public int InsertOrderDetails()
        {
            return Execute("INSERT INTO OrderDetails (DetailID ,OrderID ,ProductName ,Quantity ,Price)\r\n VALUES (1 ,1 ,'Produto teste' ,1 ,10.0)");
        }

        [HttpPost("DeleteOrderHeader")]
        public int DeleteOrderHeader()
        {
            return Execute("Delete OrderHeader");
        }

        [HttpPost("DeleteOrderDetails")]
        public int DeleteOrderDetails()
        {
            return Execute("Delete OrderDetails");
        }


        private Guid GetTransactionId() => new Guid(Request.Headers["x-transactionid"].ToString());

        int Execute(string query,
            SqlParameter[]? parameters = null)
        {
            int output = 0;

            try
            {
                _unitOfWork.BeginTransaction(GetTransactionId());

                using (SqlCommand command = new SqlCommand(query, _unitOfWork.GetConnection(GetTransactionId()),
                    _unitOfWork?.GetTransaction(GetTransactionId())))
                {
                    if (parameters != null)
                        command.Parameters.AddRange(parameters);

                    output = command.ExecuteNonQuery();
                }

                _unitOfWork?.CommitTransaction(GetTransactionId());

                return output;

            }
            catch (Exception ex)
            {
                _unitOfWork.RollbackTransaction(GetTransactionId());

                return output;
            }
            finally
            {
                _unitOfWork.Dispose(GetTransactionId());
               
            }
        }
    }
}