using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using WebAPITransaction.Transaction;

namespace WebAPITransaction.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class OrderController(IUnitOfWork _unitOfWork) : BaseController
    {
        [HttpPost("InsertOrderHeader")]
        public async Task<ActionResult<int>> InsertOrderHeaderAsync()
        {
            try
            {
                return await ExecuteScalarAsync("INSERT INTO OrderHeader (OrderID ,CustomerName ,OrderDate) VALUES (1 ,'CustomerName teste' ,'2024-01-01')");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("InsertOrderDetails")]
        public async Task<ActionResult<int>> InsertOrderDetailsAsync()
        {
            try
            {
                return await ExecuteScalarAsync("INSERT INTO OrderDetails (DetailID ,OrderID ,ProductName ,Quantity ,Price)\r\n VALUES (1 ,1 ,'Produto teste' ,1 ,10.0)");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("DeleteOrderHeader")]
        public async Task<ActionResult<int>> DeleteOrderHeaderAsync()
        {
            try
            {
                return await ExecuteScalarAsync("Delete OrderHeader");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("DeleteOrderDetails")]
        public async Task<ActionResult<int>> DeleteOrderDetailsAsync()
        {
            try
            {
                return await ExecuteScalarAsync("Delete OrderDetails");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        public async Task<int> ExecuteScalarAsync(string query,
            SqlParameter[]? parameters = null)
        {

            try
            {
                await _unitOfWork.BeginTransactionAsync(GetTransactionId());

                int output = (await _unitOfWork.GetInstanceAsync<SqlConnection>(GetTransactionId())).ExecuteScalar<int>(query, parameters);

                await _unitOfWork.CommitTransactionAsync(GetTransactionId());

                return output;

            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransactionAsync(GetTransactionId());

                throw;
            }
        }
    }
}