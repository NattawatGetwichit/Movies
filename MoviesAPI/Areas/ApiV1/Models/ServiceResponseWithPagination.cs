using MoviesAPI.Areas.ApiV1.Models;
using MoviesAPI.Areas.ApiV1.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviesAPI.Areas.ApiV1.Models
{
    public class ServiceResponseWithPagination<T> : ServiceResponse<T>
    {
        public double TotalAmountRecords { get; set; }
        public double TotalAmountPages { get; set; }
        public double CurrentPage { get; set; }
        public double RecordsPerPage { get; set; }
    }

    public static class ResponseResultWithPagination
    {
        public static ServiceResponseWithPagination<T> Success<T>(T data, PaginationResultDto paginationResult, string message = "")
        {
            return new ServiceResponseWithPagination<T>
            {
                Data = data,
                Message = message,
                TotalAmountRecords = paginationResult.TotalAmountRecords,
                TotalAmountPages = paginationResult.TotalAmountPages,
                CurrentPage = paginationResult.CurrentPage,
                RecordsPerPage = paginationResult.RecordsPerPage
            };
        }

        public static ServiceResponseWithPagination<T> Failure<T>(string message) where T : class
        {
            return new ServiceResponseWithPagination<T>
            {
                Data = null,
                IsSuccess = false,
                Message = message
            };
        }
    }
}