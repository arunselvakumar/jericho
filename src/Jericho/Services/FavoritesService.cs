namespace Jericho.Services
{
    using Microsoft.Extensions.Options;
    using Jericho.Options;
    using MongoDB.Driver;
    using MongoDB.Bson;
    using Jericho.Helpers.Interfaces;

    public class FavoritesService
    {
        private readonly MongoDbOptions mongoDbOptions;
        private readonly IMongoDatabase mongoDbInstance;

        public FavoritesService (IOptions<MongoDbOptions> MongoDbConfig, IMongoHelper mongoHelper)
        {
            this.mongoDbOptions = MongoDbConfig.Value;
            this.mongoDbInstance = mongoHelper.MongoDbInstance;
        }
    }
}