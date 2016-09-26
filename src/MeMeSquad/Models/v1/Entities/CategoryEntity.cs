namespace Jericho.Models.v1.Entities
{
    using System;
    using Newtonsoft.Json;

    public class CategoryEntity
    {
        [JsonProperty(PropertyName = "id")]
        public Guid Id { get; set; }

        public string Name { get; set; }

        public bool IsActive { get; set; }

        public DateTime Version { get; set; }
    }
}
