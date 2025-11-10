using LibaryWithBorrowAspMvc.Models.Dtos.Book;
using LibaryWithBorrowAspMvc.Models.Entities;

namespace LibaryWithBorrowAspMvc.Extension
{
    public static class BookExtensions
    {
        public static BookListAdminDto AsAdminDto(this Book book)
        {
            return new BookListAdminDto()
            {
                 Id = book.Id,
                 Title = book.Title,
                 Description = book.Description,
                 IsBorrowed = book.IsBorrowed,
                 CreateAtByAdmin = book.CreatedAtByAdmin,
                 // Won't work here
                 CategoryName = book.Category != null ? book.Category.Name : "N/A"
            };
        }
    }
}
