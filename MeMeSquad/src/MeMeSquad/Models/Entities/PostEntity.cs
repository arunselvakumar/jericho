using MeMeSquad.Models.Entities.Enums;

namespace MeMeSquad.Models.Entities
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;

    public class PostEntity
    {
        #region Properties

        [JsonProperty(PropertyName = "id")]
        public Guid Id { get; set; }

        public PostTypeEnum Type { get; set; }

        public Dictionary<char, string> Content { get; set; }

        public Guid CategoryId { get; set; }

        public IEnumerable<string> Tags { get; set; }

        public bool IsActive { get; set; }

        public long UpVotes { get; set; }

        public long DownVotes { get; set; }

        public Guid PostedBy { get; set; }

        public DateTime Version { get; set; }
        #endregion

        #region Methods

        public override string ToString()
        {
            return this.Id.ToString();
        }
        #endregion
    }
}