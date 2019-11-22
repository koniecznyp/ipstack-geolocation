using System;
using System.Net;

namespace Domain.Exceptions
{
    public abstract class AppException : Exception
    {
        public HttpStatusCode Code { get; }

        protected AppException()
        {
        }

        protected AppException(HttpStatusCode code)
        {
            Code = code;
        }

        protected AppException(string message, params object[] args) : this(HttpStatusCode.InternalServerError, message, args)
        {
        }

        protected AppException(HttpStatusCode code, string message, params object[] args) : this(null, code, message, args)
        {
        }

        protected AppException(Exception innerException, string message, params object[] args)
            : this(innerException, HttpStatusCode.InternalServerError, message, args)
        {
        }

        protected AppException(Exception innerException, HttpStatusCode code, string message, params object[] args)
            : base(string.Format(message, args), innerException)
        {
            Code = code;
        }        
    }
}