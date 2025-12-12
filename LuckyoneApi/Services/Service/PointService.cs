using LuckyoneApi.Data;
using LuckyoneApi.Entity;
using Microsoft.EntityFrameworkCore;

namespace LuckyoneApi.Services.Service
{
    public class PointService
    {

        private readonly AppDbContext _context;

        public PointService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<bool> DeductPoints(int userId, decimal points, string transactionFor, int? referenceId = null)
        { 
            
          //  using var transaction = _context.Database.BeginTransaction();
            try
            {
                // Get user's point wallet
                var wallet = await (from pw in _context.PointWallets
                                    where pw.UserId == userId
                                    select pw).FirstOrDefaultAsync();

                if (wallet == null || wallet.AvailablePoints < points)
                    return false;

                wallet.AvailablePoints -= points;
                wallet.UpdatedAt = DateTime.UtcNow;

                _context.PointWallets.Update(wallet);

                // Create transaction record
                var pointTransaction = new PointTransaction
                {
                    UserId = userId,
                    TransactionType = "Debit",
                    Points = points,
                    TransactionFor = transactionFor,
                    ReferenceId = referenceId,
                    Remarks = $"Points deducted for {transactionFor}",
                    TransactionDate = DateTime.UtcNow
                };

                await _context.PointTransactions.AddAsync(pointTransaction);

                await _context.SaveChangesAsync();

               // await transaction.CommitAsync();

                return true;


            }
            catch (Exception)
            {
               //await transaction.RollbackAsync();
                return false;
            }



        }


        public async Task<bool> CreditPoints(int userId, decimal points, string transactionFor, int? referenceId = null)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Get or create user's point wallet
                var wallet = await (from pw in _context.PointWallets
                                    where pw.UserId == userId
                                    select pw).FirstOrDefaultAsync();

                if (wallet == null)
                {
                    wallet = new PointWallet
                    {
                        UserId = userId,
                        TotalPoints = points,
                        AvailablePoints = points,
                        LockedPoints = 0,
                        UpdatedAt = DateTime.UtcNow
                    };
                    await _context.PointWallets.AddAsync(wallet);
                }
                else
                {
                    wallet.TotalPoints += points;
                    wallet.AvailablePoints += points;
                    wallet.UpdatedAt = DateTime.UtcNow;
                    _context.PointWallets.Update(wallet);
                }

                // Create transaction record
                var pointTransaction = new PointTransaction
                {
                    UserId = userId,
                    TransactionType = "Credit",
                    Points = points,
                    TransactionFor = transactionFor,
                    ReferenceId = referenceId,
                    Remarks = $"Points credited for {transactionFor}",
                    TransactionDate = DateTime.UtcNow
                };

                await _context.PointTransactions.AddAsync(pointTransaction);

                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                return true;



            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                return false;
            }
        }

        public async Task<decimal> GetUserPointsAsync(int userId)
        {
            var wallet = await (from pw in _context.PointWallets
                                where pw.UserId == userId
                                select pw).FirstOrDefaultAsync();

            return wallet?.AvailablePoints ?? 0;
        }



    }
}
