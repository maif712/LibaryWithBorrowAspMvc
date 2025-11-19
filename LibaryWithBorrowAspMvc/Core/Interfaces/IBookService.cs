using Domain.Common;
using Domain.Common.Responses;
using LibaryWithBorrowAspMvc.Models.Dtos.Book;
using LibaryWithBorrowAspMvc.Models.Entities;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace LibaryWithBorrowAspMvc.Core.Interfaces
{
    public interface IBookService
    {
        Task<PagedResult<TProjection>> GetAllAsync<TProjection>(Expression<Func<Book, TProjection>> selector, PaginationOptions options);

        Task<TProjection?> GetByIdAsync<TProjection>(Guid id, Expression<Func<Book, TProjection>> selector);
        Task<OperationResult<Book>> CreateAsync(Book entity);

        Task<OperationResult<Book>> DeleteAsync(Guid id);

        Task<OperationResult<Book>> UpdateAsync(Guid id, UpdateBookDto dto, IFormFile? imageFile);

    }
}
