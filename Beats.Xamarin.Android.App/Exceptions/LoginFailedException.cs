using System;

namespace Beats.Xamarin.Android.App.Exceptions
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