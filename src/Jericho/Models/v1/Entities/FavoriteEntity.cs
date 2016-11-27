namespace Jericho.Models.v1.Entities
{
    using System;

    using Jericho.Models.v1.Entities.Enums;

    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;

    using Newtonsoft.Json;

    public sealed class FavoriteEntity
    {
        [BsonId]
        [JsonProperty(PropertyName = "id")]
        public ObjectId Id { get; set; }

        public string ParentId { get; set; }

        public string Name { get; set; }

        public FavoriteTypeEnum FavoriteType { get; set; }

        public string Content { get; set; }

        public bool IsDeleted { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}