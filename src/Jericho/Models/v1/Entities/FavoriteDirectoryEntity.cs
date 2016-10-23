namespace Jericho.Models.v1.Entities
{
    using System;
    using Newtonsoft.Json;

    public sealed class FavoriteDirectoryEntity
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        public string Name { get; set; }

        public DateTime CreatedOn { get; set; }

        public string CreatedBy { get; set; }
    }
}