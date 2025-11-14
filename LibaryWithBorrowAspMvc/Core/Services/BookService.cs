using Domain.Common;
using Domain.Common.Responses;
using LibaryWithBorrowAspMvc.Core.Interfaces;
using LibaryWithBorrowAspMvc.Data;
using LibaryWithBorrowAspMvc.Models.Dtos.Book;
using LibaryWithBorrowAspMvc.Models.Entities;
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

        public async Task<OperationResult<Book>> DeleteAsync(Guid id)
        {
            try
            {
                var removed = await _repo.DeleteAsync(id);
                if (removed == null)
                    return OperationResult<Book>.Fail("Book not found.");

                return OperationResult<Book>.Ok(removed, "Book deleted successfully.");
            }
            catch (Exception ex)
            {
                return OperationResult<Book>.Fail($"Failed to delete entity: {ex.Message}");
            }
        }

    }
}
