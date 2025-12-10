using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LuckyoneApi.Entity
{
    public class Contest
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ContestId { get; set; }
        public string ContestName { get; set; }
        public string ContestCode { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public DateTime? DrawTime { get; set; }
        public int EntryPoints { get; set; }
        public int TotalSlots { get; set; }
        public int FilledSlots { get; set; }
        public int WinningPoints { get; set; }
        public string Status { get; set; } // "Scheduled", "Live", "Completed", "Cancelled"
        public int CreatedBy { get; set; } // Admin UserId
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }
    }
}
