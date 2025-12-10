using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LuckyoneApi.Entity
{
    public class ContestWinner
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int WinnerId { get; set; }
        public int ContestId { get; set; }
        public int UserId { get; set; }
        public int ContestDrawId { get; set; }
        public int WonPoints { get; set; }
        public DateTime WonAt { get; set; }
    }
}
