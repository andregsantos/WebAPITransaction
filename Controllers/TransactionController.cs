using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace WebAPITransaction.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TransactionController(IUnitOfWork _unitOfWork) : ControllerBase
    {
        private Guid GetTransactionId() => new Guid(Request.Headers["x-transactionid"].ToString());

        [HttpPost("BeginTransaction")]
        public void BeginTransaction() => _unitOfWork.BeginTransaction(GetTransactionId());

        [HttpPost("CommitTransaction")]
        public void CommitTransaction() => _unitOfWork.CommitTransaction(GetTransactionId());

        [HttpPost("RollbackTransaction")]
        public void RollbackTransaction() => _unitOfWork.RollbackTransaction(GetTransactionId());
    }
}
