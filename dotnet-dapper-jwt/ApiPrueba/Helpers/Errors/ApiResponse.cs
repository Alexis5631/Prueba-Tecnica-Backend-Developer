using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiPrueba.Helpers.Errors
{
    public class ApiResponse
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }

        public ApiResponse(int statusCode, string message = null)
        {
            StatusCode = statusCode;
            Message = message ?? GetDefaultMessage(statusCode);
        }

        private string GetDefaultMessage(int statusCode)
        {
            return statusCode switch
            {
                400 => "You have made an incorrect request.",
                401 => "Unauthorized user.",
                404 => "The resource you tried to request does not exist.",
                405 => "This HTTP method is not allowed on the server.",
                500 => "Server error. Contact the administrator XD.",
                _ => throw new NotImplementedException()
            };
        }
    }
}