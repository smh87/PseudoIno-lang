using System;

namespace Core.Exceptions
{
    /// <summary>
    /// An exception thrown when a file is wrongly encoded.
    /// </summary>
    public class EncodingNotSupportedException : Exception
    {
        /// <summary>
        /// An exception which will be thrown when a file encoding is not supported in the compiler.
        /// </summary>
        /// <param name="message">The exception message to show</param>
        /// <returns>An exception</returns>
        public EncodingNotSupportedException(string message) : base(message)
        { }
    }
}