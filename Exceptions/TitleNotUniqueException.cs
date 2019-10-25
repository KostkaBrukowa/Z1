using System;

namespace Z01.Exceptions
{
    public class TitleNotUniqueException : Exception
    {
        public TitleNotUniqueException()
        {
        }

        public TitleNotUniqueException(string message)
            : base(message)
        {
        }

        public TitleNotUniqueException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
