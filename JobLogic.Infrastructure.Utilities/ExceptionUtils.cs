using System;

namespace JobLogic.Infrastructure.Utilities
{
    public static class ExceptionUtils
    {
        /// <summary>
        /// Attempts to invoke an operation, ignoring any Exceptions that occur.
        /// A return value indicates whether the invoke succeeded.
        /// </summary>
        /// <remarks>
        /// Please consider not swallowing the exception, instead opting to handle it or simply bubble up for logging, etc.
        /// Only use this if you're 100% sure we don't need to handle the exception.
        /// </remarks>
        /// <param name="operation">Lambda that performs an operation that might throw an exception.</param>
        /// <returns>true if operation was invoked successfully; otherwise, false.</returns>
        public static bool TryInvoke(this Action operation)
        {
            if (operation == null)
            {
                throw new ArgumentNullException(nameof(operation));
            }

            try
            {
                operation.Invoke();
            }
            catch
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Attempts to invoke an operation, ignoring any Exceptions that occur.
        /// A return value indicates whether the invoke succeeded.
        /// An out parameter contains return value, or default value of TResult.
        /// </summary>
        /// <remarks>
        /// Please consider not swallowing the exception, instead opting to handle it or simply bubble up for logging, etc.
        /// Only use this if you're 100% sure we don't need to handle the exception.
        /// </remarks>
        /// <typeparam name="TResult">return type of operation.</typeparam>
        /// <param name="operation">Lambda that performs an operation that might throw an exception.</param>
        /// <param name="result">return value if operation was invoked successfully; otherwise, default(TResult).</param>
        /// <returns>true if operation was invoked successfully; otherwise, false.</returns>
        public static bool TryInvoke<TResult>(this Func<TResult> operation, out TResult result)
        {
            if (operation == null)
            {
                throw new ArgumentNullException(nameof(operation));
            }

            try
            {
                result = operation.Invoke();
            }
            catch
            {
                result = default;
                return false;
            }

            return true;
        }
    }
}