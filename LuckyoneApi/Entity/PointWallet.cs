using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LuckyoneApi.Entity
{
    public class PointWallet
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PointWalletId { get; set; }
        public int UserId { get; set; }

        [Precision(18, 2)]
        public decimal TotalPoints { get; set; }

        [Precision(18, 2)]
        public decimal AvailablePoints { get; set; }

        [Precision(18, 2)]
        public decimal LockedPoints { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
