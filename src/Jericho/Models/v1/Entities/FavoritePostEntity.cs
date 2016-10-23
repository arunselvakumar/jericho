namespace Jericho.Models.v1.Entities
{
    using System;
    using Newtonsoft.Json;

    public sealed class FavoritePostEntity
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        
        public string Name { get; set; }

        public string PostId { get; set; }

        public DateTime CreatedOn { get; set; }

        public string CreatedBy { get; set; }
    }
}