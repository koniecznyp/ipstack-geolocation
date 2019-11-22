using System;
using System.Net;
using Domain.Exceptions;

namespace Services.Exceptions
{
    public class ServiceException : AppException
    {
        public ServiceException()
        {
        }

        public ServiceException(HttpStatusCode code) : base(code)
        {
        }

        public ServiceException(string message, params object[] args) : base(HttpStatusCode.InternalServerError, message, args)
        {
        }

        public ServiceException(HttpStatusCode code, string message, params object[] args) : base(null, code, message, args)
        {
        }

        public ServiceException(Exception innerException, string message, params object[] args)
            : base(innerException, string.Empty, message, args)
        {
        }

        public ServiceException(Exception innerException, HttpStatusCode code, string message, params object[] args)
            : base(string.Format(message, args), innerException)
        {
        }          
    }
}