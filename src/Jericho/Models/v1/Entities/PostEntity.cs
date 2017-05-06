namespace Jericho.Models.v1.Entities
{
    using System;
    using System.Collections.Generic;

    using Jericho.Models.v1.Entities.Enums;
    using Jericho.Providers;
    using Jericho.Validations;
    using Jericho.Validations.Interfaces;

    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public class PostEntity : IValidatableEntity
    {
        #region Properties

        [BsonId]
        [JsonProperty(PropertyName = "id")]
        public ObjectId Id { get; set; }

        [JsonProperty(PropertyName = "url")]
        [BsonElement("url")]
        public string Url { get; set; }

        [JsonProperty(PropertyName = "title")]
        [BsonElement("title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "type")]
        [JsonConverter(typeof(StringEnumConverter))]
        [BsonRepresentation(BsonType.String)]
        [BsonElement("type")]
        public PostTypeEnum Type { get; set; }

        [JsonProperty(PropertyName = "status")]
        [JsonConverter(typeof(StringEnumConverter))]
        [BsonRepresentation(BsonType.String)]
        [BsonElement("status")]
        public PostStatusEnum Status { get; set; }

        [JsonProperty(PropertyName = "content")]
        [BsonElement("content")]
        public string Content { get; set; }

        [JsonProperty(PropertyName = "categoryid")]
        [BsonElement("categoryid")]
        public string CategoryId { get; set; }

        [JsonProperty(PropertyName = "tags")]
        [BsonElement("tags")]
        public IEnumerable<string> Tags { get; set; }        

        [JsonProperty(PropertyName = "upvotes")]
        [BsonElement("upvotes")]
        public long UpVotes { get; set; }

        [JsonProperty(PropertyName = "downvotes")]
        [BsonElement("downvotes")]
        public long DownVotes { get; set; }

        [JsonProperty(PropertyName = "postedby")]
        [BsonElement("postedby")]
        public string PostedBy { get; set; }

        [JsonProperty(PropertyName = "isdeleted")]
        [BsonElement("isdeleted")]
        public bool IsDeleted { get; set; }

        [JsonProperty(PropertyName = "createdon")]
        [BsonElement("createdon")]
        public DateTime CreatedOn { get; set; }

        #endregion

        #region Methods

        public override string ToString()
        {
            return this.Id.ToString();
        }

        public IEnumerable<Error> Validate()
        {
            var validationErrors = new List<Error>();

            this.RequiredValidationRule(nameof(this.Title), validationErrors);
            this.RequiredValidationRule(nameof(this.Content), validationErrors);
            this.RequiredEnumValidationRule(nameof(this.Type), validationErrors);

            return validationErrors;
        }

        #endregion
    }
}