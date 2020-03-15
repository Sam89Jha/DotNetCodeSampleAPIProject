using System;
using System.Net;
using System.Runtime.Serialization;

namespace CampaignsProductManager.Core.Exceptions
{
    [Serializable]
    public class CampaignsProductManagerException : Exception
    {
        public Error Error { get; set; }
        public HttpStatusCode ResponseCode { get; set; }

        public CampaignsProductManagerException(HttpStatusCode responseCode, Error error) 
            : base($"{error.ErrorCode}:{error.ErrorDescription}")
        {
            ResponseCode = responseCode;
            Error = error;
        }

        // Without this constructor, deserialization will fail
        protected CampaignsProductManagerException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
