#nullable enable

namespace Codekali.Net.Persistence.Models
{
    public class ServiceResult<T>
    {
        public bool Success { get; }
        public string Message { get; }
        public T? Data { get; }

        private ServiceResult(bool success, string message, T? data = default)
        {
            Success = success;
            Message = message;
            Data = data;
        }

        public static ServiceResult<T> Succeeded(T data, string message = "Operation successful.")
            => new(true, message, data);

        public static ServiceResult<T> Failed(string message)
            => new(false, message, default);
    }

}
