using System.Net;

namespace DogApp.CustomException
{
    public class HttpResponseException : Exception
    {
        public HttpStatusCode StatusCode { get; }

        public HttpResponseException(HttpStatusCode statusCode)
            : base($"HTTP Response Error with status code: {statusCode}")
        {
            StatusCode = statusCode;
        }

        public HttpResponseException(HttpStatusCode statusCode, string message)
            : base(message)
        {
            StatusCode = statusCode;
        }

        public HttpResponseException(HttpStatusCode statusCode, string message, Exception inner)
            : base(message, inner)
        {
            StatusCode = statusCode;
        }
    }
}
