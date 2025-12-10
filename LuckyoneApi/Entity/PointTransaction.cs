using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LuckyoneApi.Entity
{
    public class PointTransaction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TransactionId { get; set; }
        public int UserId { get; set; }
        public string TransactionType { get; set; } // "Credit", "Debit"
        public decimal Points { get; set; }
        public string TransactionFor { get; set; } // "ContestJoin", "ContestWin", "Bonus", "Refund"
        public int? ReferenceId { get; set; } // ContestId or other reference
        public string Remarks { get; set; }
        public DateTime TransactionDate { get; set; }
    }
}
