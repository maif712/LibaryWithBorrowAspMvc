using Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibaryWithBorrowAspMvc.Models.Entities
{
    [Table("Category")]
    public class Category : IEntity
    {
        [Key]
        public Guid Id { get; set ; }
        [Required, MaxLength(100)]
        public required string Name { get; set; }
        public string Description { get; set; } = string.Empty;
        // Navigation Property
        public List<Book> Books { get; set; } = new List<Book>();
    }
}
