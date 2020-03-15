namespace CampaignsProductManager.Core.Constants
{
    public static partial class Constants
    {
        public const string CertificateExtensionFilter = "*.pfx";
        public const string MessageDateFormat = "yyyy-MM-ddTHH:mm:ss.fffffff";
        public const string RecievedFrom = "recievedfrom";
        public const string RecievedThru = "recievedthru";
        public const string CorrelationId = "correlationid";
        public const string TargetId = "targetid";
        public const string SourceId = "sourceid";
        public const string MessageType = "messagetype";
        public const string MessageId = "messageid";
        public const string TimestampFormat = "o";

        public static class Serilog
        {
            public const string MinimumLevel = "Serilog:MinimumLevel";
        }
    }
}
