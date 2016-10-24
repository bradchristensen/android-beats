using System;

namespace Beats.Xamarin.WebApiClient.Exceptions
{
    public class SessionTimeoutException : Exception
    {
        public SessionTimeoutException()
            : base()
        {
        }

        public SessionTimeoutException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}