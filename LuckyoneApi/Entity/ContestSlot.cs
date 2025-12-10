using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LuckyoneApi.Entity
{
    public class ContestSlot
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SlotId { get; set; }
        public int ContestId { get; set; }
        public int UserId { get; set; }
        public string SlotNumber { get; set; }
        public DateTime JoinedAt { get; set; }
        public bool IsWinner { get; set; }
    }
}
