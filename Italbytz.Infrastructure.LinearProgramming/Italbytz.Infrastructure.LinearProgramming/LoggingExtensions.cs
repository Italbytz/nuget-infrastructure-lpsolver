using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Italbytz.Infrastructure.LinearProgramming
{
    public static class LoggingExtensions
    {
        private static ILogger _lpLogger = NullLogger.Instance;
        
        public static void InitLoggers(ILoggerFactory loggerFactory)
        {
            _lpLogger = loggerFactory.CreateLogger("LP");
        }
        
        public static void Log(this LpSolver lp, LogLevel logLevel,
            string message)
        {
            _lpLogger.Log(logLevel, message);
        }
    }
}