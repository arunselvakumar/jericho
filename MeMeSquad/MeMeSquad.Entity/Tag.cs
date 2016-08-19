namespace MeMeSquad.Entity
{
    using Newtonsoft.Json;
    using System;

    public class Tag
    {
        [JsonProperty(PropertyName = "id")]
        public Guid Id { get; set; }

        public Guid PostId { get; set; }

        public DateTime Version { get; set; }
    }
}
