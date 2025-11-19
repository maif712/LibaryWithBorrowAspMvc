using System.ComponentModel.DataAnnotations;

namespace LibaryWithBorrowAspMvc.Models.Dtos.Book
{
    public class UpdateBookDto
    {
        public Guid Id { get; set; }
        [Required, MaxLength(100)]
        public required string Title { get; set; }
        [Required, MaxLength(500)]
        public required string Description { get; set; }
        [Required, MaxLength(100)]
        public required string AuthorName { get; set; }
        [Required]
        public required int PublishedAt { get; set; }
        [Required]
        public required int Pages { get; set; }

        public Guid CategoryId { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
    }
}
