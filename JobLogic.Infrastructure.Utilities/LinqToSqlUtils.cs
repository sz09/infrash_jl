using System;
using System.Linq;
using System.Linq.Expressions;

namespace JobLogic.Infrastructure.Utilities
{
    public static class LinqToSqlUtils
    {
        /// <summary>
        /// Validate then paginate
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize">minimum value: 5, maximum value: 50</param>
    /// <returns></returns>
        public static IQueryable<T> Paginate<T>(this IQueryable<T> items, int pageNumber, int pageSize)
        {
            if (pageSize == int.MaxValue)
            {
                return items;
            }
            else
            {
                pageSize = pageSize.ValidatePageSize();
                pageNumber = pageNumber.ValidatePageIndex();

                return items
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize);
            }
        }

        public static IQueryable<T> FilterBySearchTerm<T>(this IQueryable<T> items, string searchTerm, Func<IQueryable<T>, string, IQueryable<T>> func, bool searchExact = true)
        {
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                searchTerm = searchTerm.ToLower();
                var filters = SearchTermUtils.Instance.Build(searchTerm, searchExact);

                if (filters != null && filters.Any())
                {
                    items = filters.Aggregate(items, func);
                }
            }

            return items;
        }

        public static IQueryable<T> FilterWithExpression<T>(this IQueryable<T> items,
            Expression<Func<T, bool>> expression)
        {
            return items.Where(expression);
        }
    }
}