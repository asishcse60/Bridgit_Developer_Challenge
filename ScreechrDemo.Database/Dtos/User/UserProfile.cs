using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ScreechrDemo.Contracts.Constants;
using ScreechrDemo.Databases.Dtos.Screechr;

namespace ScreechrDemo.Databases.Dtos.User
{
    [Table("UserProfile")]
    public class UserProfile
    {
        public UserProfile()
        {
            CreatedAt = DateTime.UtcNow;
        }

        // DatabaseGenerated(databaseGeneratedOption:DatabaseGeneratedOption.Computed)
        [Key]
        public ulong Id { get; init; }
        
        [MaxLength(FieldLimit.MAX_USER_NAME_LENGTH), Required]
        public string UserName { get; set; }

        [MaxLength(FieldLimit.MAX_FIRST_NAME_LENGTH), Required]
        public string FirstName { get; set; }

        [MaxLength(FieldLimit.MAX_LAST_NAME_LENGTH), Required]
        public string LastName { get; set; }

        [StringLength(FieldLimit.EXACT_TOKEN_LENGTH), Required]
        public string SecretToken { get; init; }
        [MaxLength(256)]
        public string ProfileImageUri { get; set; }
        [Required]
        public DateTime CreatedAt { get; init; }
        public DateTime ModifiedDate { get; set; }
        public ICollection<Screech> Screeches { get; set; }
    }
}
