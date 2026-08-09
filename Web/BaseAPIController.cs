using Codekali.Net.Persistence.Mapping;
using Codekali.Net.Persistence.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace Codekali.Net.Persistence.Web
{
    public class BaseAPIController(ILogger logger, IObjectMapper mapper) : Controller
    {
        protected ILogger Logger { get; private set; } = logger;
        protected IObjectMapper Mapper { get; private set; } = mapper;
        protected string RequestUrl
        {
            get
            {
                string url = string.Concat(this.Request.Scheme, "://", this.Request.Host, this.Request.Path, this.Request.QueryString);
                return url;
            }
        }

        public BaseAPIController() : this(null, null) { }

        public BaseAPIController(ILogger logger) : this(logger, null) { }

        protected string GetErrors()
        {
            return string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(x => x.ErrorMessage));
        }

        /// <summary>
        /// Standardized API OK Response
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="pagination"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        protected IActionResult ApiOk<T>(T data, string message = null)
        {
            var response = ApiResponse<T>.Ok(data, message);
            return Ok(response);
        }

        /// <summary>
        /// Standardized API Created Response
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="actionName"></param>
        /// <param name="routeValues"></param>
        /// <param name="data"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        protected IActionResult ApiCreatedAtAction<T>(string actionName, object routeValues, T data, string message = null)
        {
            var response = ApiResponse<T>.Created(data, message);
            return CreatedAtAction(actionName, routeValues, response);
        }

        /// <summary>
        /// Standardized API No Content Response
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        protected IActionResult ApiNoContent(string message = "No data available.")
        {
            var response = ApiResponse<object>.NoContent(message);
            return StatusCode(StatusCodes.Status204NoContent, response);
        }

        /// <summary>
        /// Standardized API Error Response with custom status code and message
        /// </summary>
        /// <param name="statuscode"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        protected IActionResult ApiError(int statuscode, string message)
        {
            var response = ApiResponse<object>.Fail(statuscode, message);
            return BadRequest(response);
        }

        /// <summary>
        /// Standardized API Unauthorized Response
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        protected IActionResult ApiUnAuthorized(string message)
        {
            var response = ApiResponse<object>.Fail(StatusCodes.Status401Unauthorized, message);
            return BadRequest(response);
        }

        /// <summary>
        /// Standardized API Bad Request Response
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        protected IActionResult ApiBadRequest(string message)
        {
            var response = ApiResponse<object>.Fail(StatusCodes.Status400BadRequest, message);
            return BadRequest(response);
        }
    }
}
