namespace Jericho.Models.v1.Entities
{
    using System;

    using Jericho.Models.v1.Entities.Enums;

    using Newtonsoft.Json;

    public sealed class FavoriteEntity
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        public string Name { get; set; }

        public FavoriteTypeEnum FavoriteType { get; set; }

        public string Content { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}