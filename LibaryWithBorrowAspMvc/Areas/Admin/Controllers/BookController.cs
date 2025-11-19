using Domain.Common;
using Humanizer;
using LibaryWithBorrowAspMvc.Core.Interfaces;
using LibaryWithBorrowAspMvc.Extension;
using LibaryWithBorrowAspMvc.Models.Dtos.Book;
using LibaryWithBorrowAspMvc.Models.Entities;
using LibaryWithBorrowAspMvc.Models.ViewModels.Book;
using LibaryWithBorrowAspMvc.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibaryWithBorrowAspMvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = StaticRoles.ADMIN)]
    public class BookController : Controller
    {
        private readonly IBookService _bookService;

        public BookController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(BookFilterOptions filter)
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
                }, new PaginationOptions { Page = filter.Page, PageSize = filter.PageSize, Descending = filter.Descending });

                BookManagementViewModel viewModel = new()
                {
                    Books = books,
                    Filter = filter
                };
                return View(viewModel);
            }
            catch (Exception)
            {
                this.AddError("Something unexpected happend!");
                return View();
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateBookDto dto, IFormFile? imageFile)
        {
            if (!ModelState.IsValid)
            {
                this.AddError("All fields are required.");
                return View(dto);
            }

            try
            {
                var newBook = new Book
                {
                    Id = Guid.CreateVersion7(),
                    Title = dto.Title,
                    Description = dto.Description,
                    AuthorName = dto.AuthorName,
                    Pages = dto.Pages,
                    PublishedAt = dto.PublishedAt,
                    CreatedAtByAdmin = DateTimeOffset.Now,
                    IsBorrowed = false,
                    CategoryId = dto.CategoryId,
                    ImageUrl = imageFile == null
                        ? "/images/no-image.png"
                        : StaticHelperFunctions.SaveImage(imageFile)
                };

                var response = await _bookService.CreateAsync(newBook);

                if (response.Success)
                {
                    this.AddSuccess(response.Message!);
                    return RedirectToAction(nameof(Index));
                }

                this.AddError(response.Message!);
                return View(dto);
            }
            catch (Exception)
            {
                this.AddError("Something unexpected happened!");
                return View(dto);
            }
        }


        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            try
            {
                var dto = await _bookService.GetByIdAsync(id, b => new UpdateBookDto
                {
                    Id = b.Id,
                    Title = b.Title,
                    AuthorName = b.AuthorName,
                    Description = b.Description,
                    PublishedAt = b.PublishedAt,
                    Pages = b.Pages,
                    CategoryId = b.CategoryId,
                    ImageUrl = b.ImageUrl
                });

                if (dto == null)
                {
                    this.AddError("Book not found.");
                    return RedirectToAction(nameof(Index));
                }

                return View(dto);
            }
            catch (Exception ex)
            {
                this.AddError($"Unexpected error while loading book: {ex.Message}");
                return RedirectToAction(nameof(Index));
            }
        }


        [HttpPost]
        public async Task<IActionResult> Edit(UpdateBookDto dto, IFormFile? imageFile)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            try
            {
                var response = await _bookService.UpdateAsync(dto.Id, dto, imageFile);

                if (response.Success)
                {
                    this.AddSuccess(response.Message!);
                    return RedirectToAction(nameof(Index));
                }

                this.AddError(response.Message!);
                return View(dto);

            }
            catch (Exception ex)
            {
                this.AddError($"Something unexpected happened: {ex.Message}");
                return View(dto);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var response = await _bookService.DeleteAsync(id);
                if (response.Success)
                {
                    this.AddSuccess(response.Message!);
                    return RedirectToAction(nameof(Index));
                }

                this.AddError(response.Message!);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {

                this.AddError("Something unexpected happened!");
                return RedirectToAction(nameof(Index));
            }
        }

    }
}
