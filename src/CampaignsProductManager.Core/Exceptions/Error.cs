using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace CampaignsProductManager.Core.Exceptions
{
    /// <summary>
    /// </summary>
    public class Error
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="Error" /> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="errorItem">The error item.</param>
        public Error(IConfiguration configuration, ErrorItem errorItem)
        {
            var errorCode = errorItem + ":" + "ErrorCode";
            ErrorCode = configuration[errorCode];
            ErrorDescription = configuration[errorItem + ":" + "ErrorDescription"];
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Error" /> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="errorItem">The error item.</param>
        /// <param name="errors">The errors.</param>
        public Error(IConfiguration configuration, ErrorItem errorItem, IEnumerable<string> errors)
        {
            ErrorCode = configuration[errorItem + ":" + "ErrorCode"];
            ErrorDescription = string.Join(';', errors);
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Error" /> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="errorItem">The error item.</param>
        /// <param name="args">The arguments.</param>
        public Error(IConfiguration configuration, ErrorItem errorItem, params object[] args)
        {
            var errorCode = errorItem + ":" + "ErrorCode";
            ErrorCode = configuration[errorCode];
            var description = configuration[errorItem + ":" + "ErrorDescription"];
            ErrorDescription = null == description ? null : string.Format(description, args);
        }

        /// <summary>
        ///     Gets or sets the error code.
        /// </summary>
        /// <value>
        ///     The error code.
        /// </value>
        public string ErrorCode { get; set; }

        /// <summary>
        ///     Gets or sets the error description.
        /// </summary>
        /// <value>
        ///     The error description.
        /// </value>
        public string ErrorDescription { get; set; }
    }
}