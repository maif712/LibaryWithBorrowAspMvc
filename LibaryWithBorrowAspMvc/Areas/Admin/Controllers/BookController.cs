using Domain.Common;
using LibaryWithBorrowAspMvc.Core.Interfaces;
using LibaryWithBorrowAspMvc.Extension;
using LibaryWithBorrowAspMvc.Models.Dtos.Book;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibaryWithBorrowAspMvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(Roles = StaticRoles.ADMIN)]
    public class BookController : Controller
    {
        private readonly IBookService _bookService;

        public BookController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int page = 1, int pageSize = 10)
        {
            try
            {
                var books = await _bookService.GetAllAsync(book => new BookListAdminDto()
                {
                    Id = book.Id,
                    Title = book.Title,
                    Description = book.Description,
                    IsBorrowed = book.IsBorrowed,
                    CreateAtByAdmin = book.CreatedAtByAdmin,
                    CategoryName = book.Category != null ? book.Category.Name : "N/A"
                }, new PaginationOptions { Page = page, PageSize = pageSize });
                return View(books);
            }
            catch (Exception)
            {
                this.AddError("Something unexpected happend!");
                return View();
            }
        }
    }
}
