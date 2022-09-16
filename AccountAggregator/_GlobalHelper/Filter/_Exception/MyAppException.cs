using System;

namespace AccountAggregator._GlobalHelper
{
    public class MyAppException : Exception
    {
        public MyAppException()
        { }

        public MyAppException(string message)
            : base(message)
        { }

        public MyAppException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
