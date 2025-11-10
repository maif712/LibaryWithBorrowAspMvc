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
        private readonly ApplicationDbContext _context;

        public BookService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<TProjection>> GetAllAsync<TProjection>(Expression<Func<Book, TProjection>> selector, PaginationOptions options)
        {
            var query = _context.Books.AsQueryable().AsNoTracking();

            var totalCount = await query.CountAsync();
            var items = await query
                .Skip((options.Page - 1) * options.PageSize)
                .Take(options.PageSize)
                .Select(selector)
                .ToListAsync();

            return new PagedResult<TProjection>()
            {
                Items = items,
                TotalCount = totalCount,
                Page = options.Page,
                PageSize = options.PageSize
            };
        }
    }
}
