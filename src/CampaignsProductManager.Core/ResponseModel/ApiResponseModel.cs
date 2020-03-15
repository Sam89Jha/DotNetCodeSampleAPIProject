using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Microsoft.AspNetCore.Http;

namespace CampaignsProductManager.Core.ResponseModel
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class ApiResponseModel<T> where T : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApiResponseModel{T}"/> class.
        /// </summary>
        public ApiResponseModel() //During Deserialization, error model is created at index 0 which is incorrect
        {
            // DO NOT USE THIS CTOR - USE THE MOST SPECIALIZED
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiResponseModel{T}"/> class.
        /// </summary>
        /// <param name="result">The result.</param>
        public ApiResponseModel(T result)
        {
            Success = true;
            Result = new ResultModel<T>(result);
            Errors = null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiResponseModel{T}"/> class.
        /// </summary>
        /// <param name="singleError">The single error.</param>
        public ApiResponseModel(ErrorModel singleError)
        {
            Success = false;
            Result = null;
            Errors = new List<ErrorModel> {singleError};
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiResponseModel{T}"/> class.
        /// </summary>
        /// <param name="listOfErrors">The list of errors.</param>
        public ApiResponseModel(IEnumerable<ErrorModel> listOfErrors)
        {
            Success = false;
            Result = null;
            Errors = new List<ErrorModel>();
            foreach (var error in listOfErrors)
            {
                Errors.Add(error);
            }
        }

        /// <summary>
        /// Gets the HTTP status for response.
        /// </summary>
        /// <value>
        /// The HTTP status for response.
        /// </value>
        [IgnoreDataMember]
        public int HttpStatusForResponse
        {
            get
            {
                var status = StatusCodes.Status200OK;
                if (Success)
                {
                    return status;
                }
                status = Errors.Any() ? Errors.First().HttpStatus : StatusCodes.Status500InternalServerError;
                return status;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="ApiResponseModel{T}"/> is success.
        /// </summary>
        /// <value>
        ///   <c>true</c> if success; otherwise, <c>false</c>.
        /// </value>
        [DataMember(Order = 10, Name = "success", IsRequired = true, EmitDefaultValue = true)]
        public bool Success { get; set; }

        /// <summary>
        /// Gets or sets the correlation identifier..
		/// An ID to track the request and response in the log files and provided back to the client.
        /// </summary>
        /// <value>
        /// The correlation identifier.
        /// </value>
        [DataMember(Order = 20, Name = "correlationId", IsRequired = false, EmitDefaultValue = true)]
        public string CorrelationId { get; set; }

        /// <summary>
        /// Gets or sets the errors.
        /// </summary>
        /// <value>
        /// The errors.
        /// </value>
        [DataMember(Order = 30, Name = "errors", IsRequired = false, EmitDefaultValue = true)]
        public List<ErrorModel> Errors { get; set; }

        /// <summary>
        /// Gets or sets the result.
        /// </summary>
        /// <value>
        /// The result.
        /// </value>
        [DataMember(Order = 40, Name = "result", IsRequired = false, EmitDefaultValue = true)]
        public ResultModel<T> Result { get; set; }
    }
}