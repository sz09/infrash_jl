using System;
using System.Collections.Generic;

namespace JobLogic.DatabaseManager
{
    public class PagedList<T> : List<T>
    {
        public PagedList(int index = 0, int pageSize = 10, int totalItemCount = 0)
            : base()
        {
            if (index < 0) throw new ArgumentOutOfRangeException("index", "Value can not be below 0.");
            if (pageSize < 1) throw new ArgumentOutOfRangeException("pageSize", "Value can not be less than 1.");
            PageSize = pageSize;
            PageIndex = index;
            TotalItemCount = totalItemCount;
        }

        public bool HasPrevious { get { return (PageIndex != 1); } }
        public bool HasNext { get { return (PageIndex < PageCount); } }
        public int PageIndex { get; private set; }
        public int PageSize { get; private set; }
        public int TotalItemCount { get; set; }
        public int PageCount { get { return (int)Math.Ceiling(TotalItemCount / (double)PageSize); } }
    }
}
