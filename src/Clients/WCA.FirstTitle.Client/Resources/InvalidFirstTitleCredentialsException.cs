using System;

namespace WCA.FirstTitle.Client
{
    public class InvalidFirstTitleCredentialsException : Exception
    {
        public FirstTitleCredential FirstTitleCredential { get; private set; }

        public InvalidFirstTitleCredentialsException()
            : base() { }
        
        public InvalidFirstTitleCredentialsException(string message, FirstTitleCredential firstTitleCredential) : base(message)
        {
            FirstTitleCredential = firstTitleCredential;
        }

        public InvalidFirstTitleCredentialsException(FirstTitleCredential firstTitleCredential)
        {
            FirstTitleCredential = firstTitleCredential;
        }

        public InvalidFirstTitleCredentialsException(string message)
            : base(message) { }

        public InvalidFirstTitleCredentialsException(string message, Exception innerException)
            : base(message, innerException) { }

    }
}
