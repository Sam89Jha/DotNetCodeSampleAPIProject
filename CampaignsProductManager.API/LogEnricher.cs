using System;
using Serilog.Core;
using Serilog.Events;

namespace CampaignsProductManager.API
{
    public class LogEnricher : ILogEventEnricher
    {
        private const string DateTimeFormat = "yyyy'-'MM'-'dd HH':'mm':'ss'.'fff'Z'";

        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("shortloglevel", GetShortLogLevel(logEvent.Level)));
            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("UTCTimestamp", logEvent.Timestamp.UtcDateTime.ToString(DateTimeFormat)));
            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("ContainerID", Environment.GetEnvironmentVariable("HOSTNAME")));
            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("ContainerHost", Environment.GetEnvironmentVariable("CONTAINER_HOST")));
        }

        private string GetShortLogLevel(LogEventLevel logLevel)
        {
            var shortLogLevel = Convert.ToString(logLevel).ToUpper();
            switch (logLevel)
            {
                case LogEventLevel.Information:
                    shortLogLevel = "INFO ";
                    break;
                case LogEventLevel.Warning:
                    shortLogLevel = "WARN ";
                    break;
                case LogEventLevel.Verbose:
                    shortLogLevel = "TRACE";
                    break;
            }
            return shortLogLevel;
        }
    }
}
