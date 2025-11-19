using Domain.Common;
using Domain.Common.Responses;
using LibaryWithBorrowAspMvc.Models.Entities;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace LibaryWithBorrowAspMvc.Core.Interfaces
{
    public interface IBookService
    {
        Task<PagedResult<TProjection>> GetAllAsync<TProjection>(
            Expression<Func<Book, TProjection>> selector,
            PaginationOptions options);
        Task<OperationResult<Book>> CreateAsync(Book entity);

        Task<OperationResult<Book>> DeleteAsync(Guid id);

    }
}
