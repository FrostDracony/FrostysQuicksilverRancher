using System.Diagnostics;

namespace FrostysQuicksilverRancher
{
    internal static class Console
    {
        [Conditional("DEBUG")]
        public static void Log(string message, bool logToFile = true)
            => SRML.Console.Console.Log(message, logToFile);

        [Conditional("DEBUG")]
        public static void LogError(string message, bool logToFile = true)
            => SRML.Console.Console.LogError(message, logToFile);
    }
}
