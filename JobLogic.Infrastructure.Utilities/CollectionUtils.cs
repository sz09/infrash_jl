using System.Collections.Generic;
using System.Linq;

namespace JobLogic.Infrastructure.Utilities
{
    public static class CollectionUtils
    {
        /// <summary>
        /// Determines whether the collection is null or contains no elements.
        /// </summary>
        /// <typeparam name="T">The IEnumerable type.</typeparam>
        /// <param name="enumerable">The enumerable, which may be null or empty.</param>
        /// <returns>
        ///     <c>true</c> if the IEnumerable is null or empty; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> enumerable)
        {
            if (enumerable == null)
                return true;

            /* If this is a list, use the Count property for efficiency. 
             * The Count property is O(1) while IEnumerable.Count() is O(N).
             *  We cast to ICollection to stop us from losing an item in non-rewindable enumerables */
            var collection = enumerable as ICollection<T>;
            if (collection != null)
                return collection.Count < 1;

            return !enumerable.Any();
        }

        public static IEnumerable<IEnumerable<T>> Batch<T>(this IEnumerable<T> items,
                                                       int maxItems)
        {
            return items.Select((item, inx) => new { item, inx })
                        .GroupBy(x => x.inx / maxItems)
                        .Select(g => g.Select(x => x.item));
        }
    }
}
