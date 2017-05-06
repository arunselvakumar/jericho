namespace Jericho.Extensions
{
    using MongoDB.Bson;

    public static class StringExtensions
    {        
        internal static BsonRegularExpression ToCaseInsensitiveRegex(this string source)
        {
            return new BsonRegularExpression("/^" + source.Replace("+", @"\+") + "$/i");
        }     
    }
}
