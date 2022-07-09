using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Back_End_Project.ViewModels
{
    public class PaginationList<T> : List<T>
    {
        public PaginationList(IQueryable<T> query, int page, int pagecount, int itemCount)
        {
            HasNext = page < pagecount;
            HasPrev = page > 1;
            PageCount = pagecount;
            Page = page;
            ItemsCount = itemCount;
            this.AddRange(query);
        }

        public int Page { get; }
        public int PageCount { get; }
        public bool HasPrev { get; }
        public bool HasNext { get; }
        public int ItemsCount { get; }

        public static PaginationList<T> Create(IQueryable<T> query, int page, int itemCount)
        {
            int pageCount = (int)Math.Ceiling((decimal)query.Count() / itemCount);

            page = page < 1 || page > pageCount ? 1 : page;

            query = query.Skip((page - 1) * itemCount).Take(itemCount);

            return new PaginationList<T>(query, page, pageCount, itemCount);
        }
    }
}
