using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviesAPI.Areas.ApiV1.DTOs
{
    public class PaginationResultDto
    {
        public double TotalAmountRecords { get; set; }
        public double TotalAmountPages { get; set; }
        public double CurrentPage { get; set; }
        public double RecordsPerPage { get; set; }
    }
}