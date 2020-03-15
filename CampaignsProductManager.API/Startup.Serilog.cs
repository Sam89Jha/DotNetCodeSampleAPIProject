using System;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Constants = CampaignsProductManager.Core.Constants.Constants;

namespace CampaignsProductManager.API
{
    public partial class Startup
    {
        private LoggingLevelSwitch _loggingLevelSwitch;

        private ILogger GetLogger()
        {
            _loggingLevelSwitch = new LoggingLevelSwitch
            {
                MinimumLevel =
                    Enum.TryParse<LogEventLevel>(Configuration[Constants.Serilog.MinimumLevel], true,
                        out var configLogLevel)
                        ? configLogLevel
                        : LogEventLevel.Information
            };

            return new LoggerConfiguration()
                .ReadFrom.Configuration(Configuration)
                .MinimumLevel.ControlledBy(_loggingLevelSwitch)
                .MinimumLevel.Override("Microsoft", _loggingLevelSwitch)
                .MinimumLevel.Override("System", _loggingLevelSwitch)
                .MinimumLevel.Override("Microsoft.AspNetCore.Authentication", _loggingLevelSwitch)
                .WriteTo.Console(
                    outputTemplate:
                    "[{Timestamp:yyyy-MM-dd HH:mm:ss.fffZ} {shortloglevel}] | {ContainerID} | {ContainerHost} | {SourceContext} | {RequestId} | {RequestPath} | {ProviderSystem} | {Message} | {NewLine}{Exception}")
                .Enrich.FromLogContext()
                .Enrich.With(new LogEnricher())
                .CreateLogger();
        }
    }
}