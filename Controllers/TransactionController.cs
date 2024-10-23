using Microsoft.AspNetCore.Mvc;
using WebAPITransaction.Transaction;

namespace WebAPITransaction.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TransactionController(IUnitOfWork _unitOfWork) : ControllerBase
    {
         Guid GetTransactionId()
        {
            if (!Guid.TryParse(Request.Headers["TransactionId"].ToString(), out var transactionId))
                throw new ArgumentException("TransactionId not found.");

            return transactionId;
        }

        [HttpPost("BeginTransaction")]
        public async Task BeginTransaction() =>  await _unitOfWork.BeginTransactionAsync(GetTransactionId());

        [HttpPost("CommitTransaction")]
        public async Task CommitTransaction() => await _unitOfWork.CommitTransactionAsync(GetTransactionId());

        [HttpPost("RollbackTransaction")]
        public async Task RollbackTransaction() => await _unitOfWork.RollbackTransactionAsync(GetTransactionId());
    }
}
