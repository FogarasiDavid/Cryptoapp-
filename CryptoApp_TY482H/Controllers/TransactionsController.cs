using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CryptoApp_TY482H.Entities;
using CryptoApp_TY482H.SqlServer;

namespace CryptoApp_TY482H.Controllers
{
    [ApiController]
    [Route("api/transactions")]
    public class TransactionsController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        public TransactionsController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet("{userId:int}")]
        public async Task<IActionResult> GetTransactions(int userId)
        {
            var transactions = await _db.Transactions
                .Where(t => t.UserId == userId)
                .OrderByDescending(t => t.Timestamp)
                .ToListAsync();
            return Ok(transactions);
        }

        [HttpGet("details/{transactionId:int}")]
        public async Task<IActionResult> GetTransactionDetails(int transactionId)
        {
            var transaction = await _db.Transactions.FindAsync(transactionId);
            return transaction != null ? Ok(transaction) : NotFound();
        }
    }
}