using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LuckyoneApi.Entity
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string Role { get; set; } // "Admin", "User"
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
    }
}
