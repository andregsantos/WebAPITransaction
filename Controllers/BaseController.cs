using Microsoft.AspNetCore.Mvc;

namespace WebAPITransaction.Controllers
{
    public class BaseController() : ControllerBase
    {
        public Guid GetTransactionId()
        {
            if (!Guid.TryParse(Request.Headers["TransactionId"].ToString(), out var transactionId))
                throw new ArgumentException("");

            return transactionId;
        }

    }
}
