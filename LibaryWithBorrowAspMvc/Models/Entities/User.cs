using Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibaryWithBorrowAspMvc.Models.Entities
{
    public enum UserRole
    {
        admin,
        user
    }

    [Table("Users")]
    public class User : IEntity
    {
        [Key]
        public Guid Id { get ; set; }

        [Required, MaxLength(100)]
        public required string UserName { get; set; }

        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        public string PasswordHashed { get; set; } = string.Empty;

        public UserRole Role { get; set; } = UserRole.user;

        public required DateTimeOffset RegisteredAt { get; set; }
    }
}
