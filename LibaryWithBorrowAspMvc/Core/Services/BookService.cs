using Domain.Common;
using Domain.Common.Responses;
using LibaryWithBorrowAspMvc.Core.Interfaces;
using LibaryWithBorrowAspMvc.Data;
using LibaryWithBorrowAspMvc.Models.Dtos.Book;
using LibaryWithBorrowAspMvc.Models.Entities;
using LibaryWithBorrowAspMvc.Utils;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace LibaryWithBorrowAspMvc.Core.Services
{
    public class BookService : IBookService
    {
        private readonly IRepository<Book> _repo;

        public BookService(IRepository<Book> repo)
        {
            _repo = repo;
        }

        public async Task<PagedResult<TProjection>> GetAllAsync<TProjection>(Expression<Func<Book, TProjection>> selector, PaginationOptions options)
            => await _repo.GetAllAsync(selector, options);

        public async Task<TProjection?> GetByIdAsync<TProjection>(Guid id, Expression<Func<Book, TProjection>> selector)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) return default;

            // Use LINQ's Select to project into the desired DTO
            return new[] { entity }.AsQueryable().Select(selector).FirstOrDefault();
        }


        public async Task<OperationResult<Book>> CreateAsync(Book entity)
        {
            try
            {
                var created = await _repo.CreateAsync(entity);
                return OperationResult<Book>.Ok(created, "Book created successfully.");
            }
            catch (Exception ex)
            {
                return OperationResult<Book>.Fail($"Failed to create book: {ex.Message}");
            }
        }

        public async Task<OperationResult<Book>> UpdateAsync(Guid id, UpdateBookDto dto, IFormFile? imageFile)
        {
            try
            {
                var existing = await _repo.GetByIdAsync(id);
                if (existing == null)
                    return OperationResult<Book>.Fail("Book not found.");

                // Update scalar properties
                existing.Title = dto.Title;
                existing.AuthorName = dto.AuthorName;
                existing.Description = dto.Description;
                existing.PublishedAt = dto.PublishedAt;
                existing.Pages = dto.Pages;
                existing.CategoryId = dto.CategoryId;

                // Handle image
                existing.ImageUrl = StaticHelperFunctions.SaveImage(imageFile!, existing.ImageUrl);

                var updated = await _repo.UpdateAsync(id, existing);
                return OperationResult<Book>.Ok(updated!, "Book updated successfully.");
            }
            catch (Exception ex)
            {
                return OperationResult<Book>.Fail($"Failed to update book: {ex.Message}");
            }
        }



        public async Task<OperationResult<Book>> DeleteAsync(Guid id)
        {
            try
            {
                var removed = await _repo.DeleteAsync(id);
                if (removed == null)
                    return OperationResult<Book>.Fail("Book not found.");

                // Remvoe the image
                StaticHelperFunctions.DeleteImage(removed.ImageUrl);

                return OperationResult<Book>.Ok(removed, "Book deleted successfully.");
            }
            catch (Exception ex)
            {
                return OperationResult<Book>.Fail($"Failed to delete entity: {ex.Message}");
            }
        }

        
    }
}
