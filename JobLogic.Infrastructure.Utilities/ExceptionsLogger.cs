using System;
using System.Diagnostics;

namespace JobLogic.Infrastructure.Utilities
{
    public static class ExceptionsLogger
    {
        public static void WriteError(string source, Exception ex)
        {
            StackTrace stackTrace = new StackTrace();
            StackFrame stackFrame = stackTrace.GetFrame(1);

            Console.WriteLine($@"Source: {source}
Method: {stackFrame.GetMethod().Name}
Message: {ex.Message}
StackTrace: {ex.StackTrace}
Date: {DateTime.UtcNow.ToString()}
-----------------------------------------------------");
        }
    }
}
