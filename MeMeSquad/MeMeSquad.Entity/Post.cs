namespace MeMeSquad.Entity
{
    using Enums;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;

    public class Post
    {
        [JsonProperty(PropertyName = "id")]
        public Guid Id { get; set; }

        public PostTypeEnum Type { get; set; }

        public Dictionary<char, string> Content { get; set; }

        public bool IsActive { get; set; }

        public long UpVotes { get; set; }

        public long DownVotes { get; set; }

        public DateTime Version { get; set; }
    }
}