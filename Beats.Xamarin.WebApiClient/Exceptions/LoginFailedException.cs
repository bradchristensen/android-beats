using System;

namespace Beats.Xamarin.WebApiClient.Exceptions
{
    public class LoginFailedException : Exception
    {
        public LoginFailedException()
            : base()
        {
        }

        public LoginFailedException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}