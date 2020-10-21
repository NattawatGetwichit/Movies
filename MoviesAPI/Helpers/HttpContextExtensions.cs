using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoviesAPI.Helpers
{
    public static class HttpContextExtensions
    {
        public async static Task InsertPaginationParametersInResponse<T>(this HttpContext httpContext, IQueryable<T> queryable, int recordsPerPage, int totalAmountRecords, int currentPage)
        {
            if (httpContext == null) { throw new ArgumentNullException(nameof(httpContext)); }

            double count = await queryable.CountAsync();
            //double count = queryable.Count();
            double totalAmountPages = Math.Ceiling(count / recordsPerPage);
            httpContext.Response.Headers.Add("totalAmountRecords", totalAmountRecords.ToString());
            httpContext.Response.Headers.Add("totalAmountPages", totalAmountPages.ToString());

            httpContext.Response.Headers.Add("currentPage", currentPage.ToString());
            httpContext.Response.Headers.Add("recordsPerPage", recordsPerPage.ToString());
        }
    }
}