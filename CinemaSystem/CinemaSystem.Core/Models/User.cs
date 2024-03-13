using System.ComponentModel.DataAnnotations.Schema;

namespace CinemaSystem.Core.Models
{
    public class User : Entity
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public int RoleId { get; set; }

        [ForeignKey("RoleId")]
        public Role Role { get; set; }

    }
}
