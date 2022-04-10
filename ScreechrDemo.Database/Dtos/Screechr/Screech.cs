using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ScreechrDemo.Contracts.Constants;
using ScreechrDemo.Databases.Dtos.User;

namespace ScreechrDemo.Databases.Dtos.Screechr
{
    [Table("Screech")]
    public class Screech
    {
        public Screech()
        {
            CreatedAt = DateTime.UtcNow;
        }
        [Key]
        public ulong Id { get; set; }

        [MaxLength(FieldLimit.MAX_CONTENT_SIZE), Required]
        public string Content { get; set; }

        [Required]
        public ulong UserProfileId { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedDate { get; set; }
        public UserProfile UserProfile { get; set; }
    }
}
