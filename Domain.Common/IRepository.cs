using Domain.Common.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Common
{
    public interface IRepository<TEntity>
    {
        Task<PagedResult<TProjection>> GetAllAsync<TProjection>(Expression<Func<TEntity, TProjection>> selector, PaginationOptions options);

        Task<TEntity> CreateAsync(TEntity entity);
        Task<TEntity?> DeleteAsync(Guid id);
    }
}
