namespace CampaignsProductManager.Core.ResponseModel
{
    /// <summary>
    /// </summary>
    public sealed class ErrorModel
    {
        /// <summary>
        ///     The error code general
        /// </summary>
        public const string ErrorCodeGeneral = @"GL10000";

        /// <summary>
        ///     The error code general bad request
        /// </summary>
        public const string ErrorCodeGeneralBadRequest = @"GL10001";

        /// <summary>
        ///     The error message default
        /// </summary>
        public const string ErrorMessageDefault = @"An error occurred on the server preventing it to handle the request successfully.";

        /// <summary>
        ///     The error message default bad request
        /// </summary>
        public const string ErrorMessageDefaultBadRequest = @"Wrong or invalid data was sent to the service. Please, review it.";

        /// <summary>
        ///     The error detail default
        /// </summary>
        public const string ErrorDetailDefault = @"Check the logs for further detail.";

        /// <summary>
        ///     Initializes a new instance of the <see cref="ErrorModel" /> class.
        /// </summary>
        /// <param name="httpStatus">The HTTP status.</param>
        /// <param name="errorCode">The error code.</param>
        public ErrorModel(int httpStatus, string errorCode = ErrorCodeGeneral)
        {
            HttpStatus = httpStatus;
            ErrorCode = errorCode;
        }

        /// <summary>
        ///     Gets or sets the HTTP status.
        /// </summary>
        /// <value>
        ///     The HTTP status.
        /// </value>
        public int HttpStatus { get; set; }

        /// <summary>
        ///     Gets or sets the error code.
        /// </summary>
        /// <value>
        ///     The error code.
        /// </value>
        public string ErrorCode { get; set; }

        /// <summary>
        ///     Gets or sets the message.
        /// </summary>
        /// <value>
        ///     The message.
        /// </value>
        public string Message { get; set; }

        /// <summary>
        ///     Gets or sets the detail.
        /// </summary>
        /// <value>
        ///     The detail.
        /// </value>
        public string Detail { get; set; }

        /// <summary>
        ///     Gets or sets the error information URL.
        /// </summary>
        /// <value>
        ///     The error information URL.
        /// </value>
        public string ErrorInfoUrl { get; set; }
    }
}