using System.Runtime.Serialization;

namespace CampaignsProductManager.Core.ResponseModel
{
    /// <summary>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ResultModel<T>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ResultModel{T}" /> class.
        /// </summary>
        public ResultModel()
        {
            ModelType = typeof(T).Name;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ResultModel{T}" /> class.
        /// </summary>
        /// <param name="single">The single.</param>
        /// <param name="modelType">Type of the model.</param>
        public ResultModel(T single, string modelType = null)
        {
            Single = single;
            ModelType = string.IsNullOrWhiteSpace(modelType) ? typeof(T).Name : modelType;
        }

        /// <summary>
        ///     Gets the type of the model.
        /// </summary>
        /// <value>
        ///     The type of the model.
        /// </value>
        [DataMember(Order = 10, Name = "modelType", IsRequired = false, EmitDefaultValue = true)]
        public string ModelType { get; private set; }

        /// <summary>
        ///     Gets the single.
        /// </summary>
        /// <value>
        ///     The single.
        /// </value>
        [DataMember(Order = 20, Name = "single", IsRequired = false, EmitDefaultValue = true)]
        public T Single { get; private set; }
    }
}