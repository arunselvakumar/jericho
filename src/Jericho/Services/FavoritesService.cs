namespace Jericho.Services
{
    using System.Threading.Tasks;

    using Jericho.Helpers.Interfaces;
    using Jericho.Options;
    using Jericho.Services.Interfaces;

    using Microsoft.Extensions.Options;

    using MongoDB.Bson;
    using MongoDB.Driver;

    public class FavoritesService : IFavoritesService
    {
        private readonly MongoDbOptions mongoDbOptions;
        private readonly IMongoDatabase mongoDbInstance;

        public FavoritesService (IOptions<MongoDbOptions> MongoDbConfig, IMongoHelper mongoHelper)
        {
            this.mongoDbOptions = MongoDbConfig.Value;
            this.mongoDbInstance = mongoHelper.MongoDbInstance;
        }

        public Task GetAllFavoritesDirectoryAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task SaveFavoritesDirectoryAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task DeleteFavoritesDirectoryAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task GetPostsFromFavoritesDirectoryAsync(string directoryId)
        {
            throw new System.NotImplementedException();
        }

        public Task AddPostToFavoritesDirectoryAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task DeletePostFromFavoritesDirectoryAsync()
        {
            throw new System.NotImplementedException();
        }
    }
}