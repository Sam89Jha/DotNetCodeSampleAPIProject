using System;
using System.Runtime.Serialization;

namespace CampaignsProductManager.Core.Models
{
    [DataContract]
    public class ProductDto
    {
        private Guid _id;

        public ProductDto()
        {
            _id = Guid.NewGuid();
        }

        [DataMember]
        public string Id
        {
            get => _id.ToString();
        }

        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// ignore this field for serialization, so not decorated with DataMember
        /// </summary>
        public string CampaignId { get; set; }
    }
}
