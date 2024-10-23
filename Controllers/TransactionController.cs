using Microsoft.AspNetCore.Mvc;
using WebAPITransaction.Transaction;

namespace WebAPITransaction.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TransactionController(IUnitOfWork _unitOfWork) : BaseController
    {

        [HttpPost("BeginTransaction")]
        public async Task BeginTransaction() =>  await _unitOfWork.BeginTransactionAsync(GetTransactionId());

        [HttpPost("CommitTransaction")]
        public async Task CommitTransaction() => await _unitOfWork.CommitTransactionAsync(GetTransactionId());

        [HttpPost("RollbackTransaction")]
        public async Task RollbackTransaction() => await _unitOfWork.RollbackTransactionAsync(GetTransactionId());
    }
}
