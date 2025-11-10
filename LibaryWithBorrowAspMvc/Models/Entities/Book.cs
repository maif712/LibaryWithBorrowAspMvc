using Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibaryWithBorrowAspMvc.Models.Entities
{
    [Table("Books")]
    public class Book : IEntity
    {
        [Key]
        public Guid Id { get ; set; }
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
        public string ImageUrl { get; set; } = string.Empty;
        public required DateTimeOffset CreatedAtByAdmin { get; set; }

        public bool IsBorrowed { get; set; } = false;
        // Category FK
        public Guid CategoryId { get; set; }
        // Navigation property
        public Category? Category { get; set; }
    }
}
