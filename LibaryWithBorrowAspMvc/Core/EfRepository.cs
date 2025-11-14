using Domain.Common;
using Domain.Common.Responses;
using LibaryWithBorrowAspMvc.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace LibaryWithBorrowAspMvc.Core
{
    public class EfRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly ApplicationDbContext _context;

        public EfRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<TProjection>> GetAllAsync<TProjection>(Expression<Func<TEntity, TProjection>> selector, PaginationOptions options)
        {
            var query = _context.Set<TEntity>().AsQueryable().AsNoTracking();

            var totalCount = await query.CountAsync();

            // Handle empty dataset
            if (totalCount == 0)
            {
                return new PagedResult<TProjection>
                {
                    Items = new List<TProjection>(),
                    TotalCount = 0,
                    Page = 1,
                    PageSize = options.PageSize
                };
            }

            // Normalize page number
            var totalPages = (int)Math.Ceiling((double)totalCount / options.PageSize);
            var page = options.Page < 1 ? 1 : options.Page;
            if (page > totalPages)
                page = totalPages;

            // Apply ordering based on options
            if (options.Descending)
            {
                query = query.OrderByDescending(e => EF.Property<DateTimeOffset>(e, "CreatedAtByAdmin"));
            }
            else
            {
                query = query.OrderBy(e => EF.Property<DateTimeOffset>(e, "CreatedAtByAdmin"));
            }

            var items = await query
                .Skip((page - 1) * options.PageSize)
                .Take(options.PageSize)
                .Select(selector)
                .ToListAsync();

            return new PagedResult<TProjection>
            {
                Items = items,
                TotalCount = totalCount,
                Page = page,
                PageSize = options.PageSize
            };
        }

        public async Task<TEntity> CreateAsync(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            await _context.Set<TEntity>().AddAsync(entity);
            await _context.SaveChangesAsync();

            return entity;
        }
    }

}
