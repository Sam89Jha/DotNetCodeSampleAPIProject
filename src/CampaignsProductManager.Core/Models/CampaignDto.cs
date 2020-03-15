using System;
using System.Runtime.Serialization;

namespace CampaignsProductManager.Core.Models
{
    [DataContract]
    public class CampaignDto
    {
        private Guid _id;

        public CampaignDto()
        {
            _id = Guid.NewGuid();
        }

        [DataMember]
        public string Id
        {
            get => _id.ToString();
            set => _id = new Guid(value);
        }

        [DataMember]
        public ProductDto Product { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Start { get; set; }

        [DataMember]
        public string End { get; set; }
    }
}
