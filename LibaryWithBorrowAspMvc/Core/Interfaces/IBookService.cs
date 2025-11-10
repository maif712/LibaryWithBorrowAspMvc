using Domain.Common;
using LibaryWithBorrowAspMvc.Models.Entities;

namespace LibaryWithBorrowAspMvc.Core.Interfaces
{
    public interface IBookService : IRepository<Book>
    {
        
    }
}
