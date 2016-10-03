namespace Jericho.Models.v1.Entities
{
    using System;
    using System.Collections.Generic;

    using Jericho.Models.v1.Entities.Enums;

    using Newtonsoft.Json;
    using MongoDB.Bson.Serialization.Attributes;
    using MongoDB.Bson;

    public class PostEntity
    {
        #region Properties

        [BsonId]
        public ObjectId Id { get; set; }

        public string Url { get; set; }

        public string Title { get; set; }
            
        public PostTypeEnum Type { get; set; }

        public PostStatusEnum Status { get; set; }

        public string Content { get; set; }

        public string CategoryId { get; set; }

        public IEnumerable<string> Tags { get; set; }        

        public long UpVotes { get; set; }

        public long DownVotes { get; set; }

        public string PostedBy { get; set; }

        public bool IsDeleted { get; set; }
            
        public DateTime CreatedOn { get; set; }

        #endregion

        #region Methods

        public override string ToString()
        {
            return this.Id.ToString();
        }
        #endregion
    }
}