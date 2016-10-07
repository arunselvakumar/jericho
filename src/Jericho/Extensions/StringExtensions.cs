using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Jericho.Extensions
{
    public static class StringExtensions
    {        
        internal static BsonRegularExpression ToCaseInsensitiveRegex(this string source)
        {
            return new BsonRegularExpression("/^" + source.Replace("+", @"\+") + "$/i");
        }     
    }
}
