using System.ComponentModel.DataAnnotations;

namespace LibaryWithBorrowAspMvc.Models.Dtos.User
{
    public class RegisterUserDto
    {
        [Required, MaxLength(100)]
        public required string UserName { get; set; }

        [Required, EmailAddress]
        public required string Email { get; set; }

        [DataType(DataType.Password)]
        public required string Password { get; set; }
    }
}
