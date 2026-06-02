using CRM.Infrastructure.Shared.Pagination;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Application.Helpers
{
    public static class QueryablePagingExtensions
    {
        public static async Task<PagedResult<TDto>> ToPagedResultAsync<TEntity, TDto>(
        this IQueryable<TEntity> query,
        BaseQueryParams pagination,
        Func<TEntity, TDto> map)
        where TEntity : class
        {
            if (pagination.PageNumber < 1)
                pagination.PageNumber = 1;

            var totalCount = await query.CountAsync();

            var skip = (pagination.PageNumber - 1) * pagination.PageSize;

            var pageEntities = await query
                .Skip(skip)
                .Take(pagination.PageSize)
                .ToListAsync();

            var items = pageEntities.Select(map).ToList();

            var totalPages = (int)Math.Ceiling(totalCount / (double)pagination.PageSize);

            return new PagedResult<TDto>
            {
                Items = items,
                PageNumber = pagination.PageNumber,
                PageSize = pagination.PageSize,
                TotalCount = totalCount,
                TotalPages = totalPages,
                HasPreviousPage = pagination.PageNumber > 1,
                HasNextPage = pagination.PageNumber < totalPages
            };
        }
    }
}
