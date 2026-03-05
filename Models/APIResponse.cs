using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

#nullable enable

namespace GenericFunctions.Models
{
    public class ApiResponse<T>(bool success, int statusCode, string message, T? data = default)
    {
        public bool Success { get; set; } = success;
        public int StatusCode { get; set; } = statusCode;
        public string Message { get; set; } = message;
        public T? Data { get; set; } = data;

        public static ApiResponse<T> Ok(T data, string message = "Data Fetch Successful!")
            => new(true, StatusCodes.Status200OK, message, data);

        public static ApiResponse<T> Created(T data, string message = "Resource Created Successfully!")
            => new(true, StatusCodes.Status201Created, message, data);

        public static ApiResponse<T> Fail(int statusCode, string message)
            => new(false, statusCode, message);

        public static ApiResponse<T> NoContent(string message = "No data available.")
            => new(true, StatusCodes.Status204NoContent, message);
    }

    public class PaginationMetadata
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int SkipRows => (PageNumber - 1) * PageSize;
        public string? SearchTerm { get; set; }
    }

    public class PagedResult<T>(List<T> items, PaginationMetadata pagination, int totalCount)
    {
        public int TotalCount { get; set; } = totalCount;
        public PaginationMetadata Pagination { get; set; } = pagination;
        public List<T> Items { get; set; } = items;
    }
}
