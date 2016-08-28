namespace MeMeSquad.Models
{
    using Newtonsoft.Json;
    using System;

    public class Category
    {
        [JsonProperty(PropertyName = "id")]
        public Guid Id { get; set; }

        public string Name { get; set; }

        public bool IsActive { get; set; }

        public DateTime Version { get; set; }
    }
}
