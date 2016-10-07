using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Jericho.Extensions
{
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
