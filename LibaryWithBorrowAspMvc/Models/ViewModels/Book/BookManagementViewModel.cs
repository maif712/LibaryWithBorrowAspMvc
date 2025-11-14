using Domain.Common.Responses;
using LibaryWithBorrowAspMvc.Models.Dtos.Book;

namespace LibaryWithBorrowAspMvc.Models.ViewModels.Book
{
    public class BookManagementViewModel
    {
        public PagedResult<BookListAdminDto> Books { get; set; } = new PagedResult<BookListAdminDto>();
        public BookFilterOptions Filter { get; set; } = new BookFilterOptions();
    }
}
