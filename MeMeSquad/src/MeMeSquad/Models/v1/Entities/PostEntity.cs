namespace MeMeSquad.Models.v1.Entities
{
    using System;
    using System.Collections.Generic;

    using MeMeSquad.Models.v1.Entities.Enums;

    using Newtonsoft.Json;
    using MongoDB.Bson.Serialization.Attributes;
    using MongoDB.Bson;

    public class PostEntity
    {
        #region Properties

        [BsonId]
        public ObjectId Id { get; set; }

        public PostTypeEnum Type { get; set; }

        public Dictionary<char, string> Content { get; set; }

        public string CategoryId { get; set; }

        public IEnumerable<string> Tags { get; set; }

        public bool IsActive { get; set; }

        public long UpVotes { get; set; }

        public long DownVotes { get; set; }

        public string PostedBy { get; set; }

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