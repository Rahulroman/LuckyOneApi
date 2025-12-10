using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LuckyoneApi.Entity
{
    public class PointWallet
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PointWalletId { get; set; }
        public int UserId { get; set; }
        public decimal TotalPoints { get; set; }
        public decimal AvailablePoints { get; set; }
        public decimal LockedPoints { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
