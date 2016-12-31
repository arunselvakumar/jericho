using Jericho.Models.v1.Entities.Enums;

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

using Newtonsoft.Json;

namespace Jericho.Models.v1.DTOs.Favorite
{
    public class GetFavoriteResponseDto
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        public string ParentId { get; set; }

        public string Name { get; set; }

        public string FavoriteType { get; set; }

        public string Content { get; set; }
    }        

}