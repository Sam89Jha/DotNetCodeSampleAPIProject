using Serilog;
using Serilog.Events;

namespace CampaignsProductManager.Core.Extensions
{
    public static class LoggerExtensions
    {
        /// <summary>
        /// Determines whether verbose level is enabled.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="message"></param>
        public static void LogVerbose(this ILogger logger, string message)
        {
            if (logger.IsEnabled(LogEventLevel.Verbose))
            {
                logger.Verbose(message);
            }
        }
    }
}
