using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LuckyoneApi.Entity
{
    public class ContestDraw
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int DrawId { get; set; }
        public int ContestId { get; set; }
        public DateTime DrawTime { get; set; }
        public string WinningSlotNumber { get; set; }
        public int? WinnerUserId { get; set; }
        public string Status { get; set; } // "Pending", "Completed", "Cancelled"
        public int DeclaredBy { get; set; } // Admin UserId
        public DateTime? DeclaredAt { get; set; }
    }
}
