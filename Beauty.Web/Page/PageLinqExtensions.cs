using System.Collections.Generic;
/*
 ASP.NET MvcPager control
 Copyright:2009-2010 Webdiyer (http://en.webdiyer.com)
 Source code released under Ms-PL license
*/
using System.Linq;
using Beauty.Core;

namespace Beauty.Web.Page
{
    public static class PageLinqExtensions
    {
        public static PagedList<T> ToPagedList<T>
            (
                this IQueryable<T> allItems,
                int pageIndex,
                int pageSize
            )
        {
            if (pageIndex < 1)
                pageIndex = 1;
            var itemIndex = (pageIndex-1) * pageSize;
            var pageOfItems = allItems.Skip(itemIndex).Take(pageSize);
            var totalItemCount = allItems.Count();
            return new PagedList<T>(pageOfItems, pageIndex, pageSize, totalItemCount);
        }

        public static PagedList<T> ToPagedList<T>
            (
                this IList<T> allItems,
                PaginationInfo pageinfo
            )
        {
            var pageIndex = pageinfo.Current;
            var itemIndex = (pageIndex - 1) * pageinfo.Size;
            var pageOfItems = allItems;
            var totalItemCount = pageinfo.TotalRecords;
            return new PagedList<T>(pageOfItems, pageIndex, pageinfo.Size, totalItemCount);
        }
    }
}
