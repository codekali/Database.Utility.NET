using GenericFunctions.Web.Filters;
using System;
using System.Net;

namespace GenericFunctions.Web.Utility
{
    public class ExceptionsMapping
    {
        public static ProblemDetails Map(Exception ex)
        {
            ProblemDetails apiError;
            if (ex is ArgumentException)
            {
                // API exceptions are mapped to HTTP 400 BadRequest
                var exception = ex as ArgumentException;
                apiError = new ProblemDetails()
                {
                    Status = (int)HttpStatusCode.BadRequest,
                    Title = "Bad Request",
                    Detail = exception.Message
                };
            }
            else if (ex is UnauthorizedAccessException)
            {
                // Unauthorized access is mapped to HTTP 403 Forbidden
                apiError = new ProblemDetails()
                {
                    Status = (int)HttpStatusCode.Forbidden,
                    Title = "Unauthorized access",
                    Detail = ex.Message
                };
            }
            else
            {
                // By default, errors are mapped to HTTP 500 Internal Server Error
                var exception = ex;
                apiError = new ProblemDetails()
                {
                    Status = (int)HttpStatusCode.InternalServerError,
                    Title = "Internal Error",
                    Detail = "An unexpected error occurred."
                };
            }

            return apiError;
        }
    }
}
