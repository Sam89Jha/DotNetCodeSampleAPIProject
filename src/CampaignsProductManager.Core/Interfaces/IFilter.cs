using System.Collections.Generic;
using Microsoft.Extensions.Primitives;

namespace CampaignsProductManager.Core.Interfaces
{
    /// <summary>
    /// </summary>
    public interface IFilter
    {
        /// <summary>
        ///     Gets or sets the sort by.
        /// </summary>
        /// <value>
        ///     The sort by.
        /// </value>
        string SortBy { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether [sort desc].
        /// </summary>
        /// <value>
        ///     <c>true</c> if [sort desc]; otherwise, <c>false</c>.
        /// </value>
        bool SortDesc { get; set; }

        /// <summary>
        ///     Gets or sets the skip.
        /// </summary>
        /// <value>
        ///     The skip.
        /// </value>
        int Skip { get; set; }

        /// <summary>
        ///     Gets or sets the top.
        /// </summary>
        /// <value>
        ///     The top.
        /// </value>
        int Top { get; set; }

        /// <summary>
        /// extract query string from request Url
        /// </summary>
        /// <param name="requestUri"></param>
        /// <returns></returns>
        Dictionary<string, StringValues> GetQueryParameters(string requestUri);
    }
}