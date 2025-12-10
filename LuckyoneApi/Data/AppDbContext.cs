using LuckyoneApi.Entity;
using Microsoft.EntityFrameworkCore;
using System;

namespace LuckyoneApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
              : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Contest> Contests { get; set; }
        public DbSet<ContestSlot> ContestSlots { get; set; }
        public DbSet<ContestDraw> ContestDraws { get; set; }
        public DbSet<ContestWinner> ContestWinners { get; set; }
        public DbSet<PointWallet> PointWallets { get; set; }
        public DbSet<PointTransaction> PointTransactions { get; set; }


    }
}
