namespace Jericho.Extensions
{
    using MongoDB.Bson;

    public static class BsonDocumentExtensions
    {
        public static BsonDocument RemoveDefaultPostFilterPresets(this BsonDocument filter)
        {
            filter.Remove("page");
            filter.Remove("limit");
            filter.Remove("status");
            filter.Remove("isdeleted");

            return filter;
        }

        public static BsonDocument ApplyDefaultPostFilterPresets(this BsonDocument filter)
        {
            filter.Add(new BsonElement("status", "Approved"));
            filter.Add(new BsonElement("isdeleted", false));

            return filter;
        }
    }
}
