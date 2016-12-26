namespace Jericho.Services
{
    using System.Threading.Tasks;

    using Jericho.Helpers.Interfaces;
    using Jericho.Models.v1.Entities;
    using Jericho.Options;
    using Jericho.Providers.ServiceResultProvider;
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

        public async Task<ServiceResult<FavoriteEntity>> SaveFavoritesDirectoryAsync(FavoriteEntity entity)
        {
            var favoritesCollection = this.mongoDbInstance.GetCollection<FavoriteEntity>(this.mongoDbOptions.FavoritesCollectionName);
            await favoritesCollection.InsertOneAsync(entity);

            var insertedEntity = await this.GetFavoriteEntityById(entity.Id.ToString());

            return new ServiceResult<FavoriteEntity>(true, insertedEntity);
        }

        public async Task<ServiceResult<object>> DeleteFavoritesDirectoryAsync(string id)
        {
            var entity = await this.GetFavoriteEntityById(id);
            entity.IsDeleted = true;

            var result = await this.UpdateFavoriteEntity(entity);
            return result == null ? new ServiceResult<object>(false) : new ServiceResult<object>(true);
        }

        public async Task<ServiceResult<object>> GetPostsFromFavoritesDirectoryAsync(string directoryId)
        {
            var favoritesCollection = this.mongoDbInstance.GetCollection<FavoriteEntity>(this.mongoDbOptions.FavoritesCollectionName);
            var result = favoritesCollection.Find(filter => filter.ParentId.Equals(directoryId));
            return result == null ? new ServiceResult<object>(false) : new ServiceResult<object>(true, await result.ToListAsync());
        }

        public async Task<ServiceResult<FavoriteEntity>> AddPostToFavoritesDirectoryAsync(string id, FavoriteEntity entity)
        {
            entity.ParentId = id;

            var favoritesCollection = this.mongoDbInstance.GetCollection<FavoriteEntity>(this.mongoDbOptions.FavoritesCollectionName);
            await favoritesCollection.InsertOneAsync(entity);

            var insertedEntity = await this.GetFavoriteEntityById(entity.Id.ToString());

            return new ServiceResult<FavoriteEntity>(true, insertedEntity);
        }

        public Task DeletePostFromFavoritesDirectoryAsync()
        {
            throw new System.NotImplementedException();
        }

        private async Task<FavoriteEntity> GetFavoriteEntityById(string id)
        {
            var favoritesCollection = this.mongoDbInstance.GetCollection<FavoriteEntity>(this.mongoDbOptions.FavoritesCollectionName);
            var favoriteEntity = await favoritesCollection.FindAsync(Builders<FavoriteEntity>.Filter.Eq("_id", ObjectId.Parse(id)));

            return await favoriteEntity.FirstOrDefaultAsync();
        }

        private async Task<FavoriteEntity> UpdateFavoriteEntity(FavoriteEntity entity)
        {
            var favoritesCollection = this.mongoDbInstance.GetCollection<FavoriteEntity>(this.mongoDbOptions.FavoritesCollectionName);
            var result = await favoritesCollection.ReplaceOneAsync(Builders<FavoriteEntity>.Filter.Eq("_id", entity.Id), entity);

            if (result.IsAcknowledged)
            {
                return await this.GetFavoriteEntityById(entity.Id.ToString());
            }

            return null;
        }
    }
}