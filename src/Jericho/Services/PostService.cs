namespace Jericho.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Jericho.Helpers.Interfaces;
    using Jericho.Models.v1.Entities;
    using Jericho.Options;
    using Jericho.Services.Interfaces;

    using System.ComponentModel.DataAnnotations;
    using Jericho.Providers.ServiceResultProvider;

    using Microsoft.Extensions.Options;

    using MongoDB.Driver;
    using MongoDB.Bson;
    using Microsoft.AspNetCore.Http;
    using Models.v1.Entities.Extensions;
    using Extensions;

    public class PostService : IPostService
    {
        private readonly MongoDbOptions mongoDbOptions;
        private readonly IMongoDatabase mongoDbInstance;

        public PostService(IOptions<MongoDbOptions> MongoDbConfig, IMongoHelper mongoHelper)
        {
            this.mongoDbInstance = mongoHelper.MongoDbInstance;
            this.mongoDbOptions = MongoDbConfig.Value;
        }

        public async Task<ServiceResult<PostEntity>> CreatePostAsync(PostEntity postEntity)
        {               
            var validationErrors = postEntity.Validate();

            if (validationErrors.Any())
            {
                return new ServiceResult<PostEntity>(false, validationErrors);
            }

            var postCollection = mongoDbInstance.GetCollection<PostEntity>(this.mongoDbOptions.PostsCollectionName);
            await postCollection.InsertOneAsync(postEntity);

            var insertedEntity = await GetPostByIdAsync(postEntity.Id.ToString());            

            return new ServiceResult<PostEntity>(true, insertedEntity);
        }

        public async Task<ServiceResult<PostEntity>> GetPostAsync(string id)
        {
            var postEntity = await this.GetPostByIdAsync(id);

            if(postEntity == null)
            {
                return new ServiceResult<PostEntity>(false);
            }

            return new ServiceResult<PostEntity>(true, postEntity);
        }

        public async Task<ServiceResult<IEnumerable<PostEntity>>> GetPostsAsync(IQueryCollection query, int page, int limit)
        {
            var filter = new BsonDocument(query.ToDictionary(kvp => kvp.Key.ToLower(), kvp => kvp.Value[0].ToString().ToCaseInsensitiveRegex()));
            filter.RemoveDefaultPostFilterPresets();
            filter.ApplyDefaultPostFilterPresets();

            var postEntities = await mongoDbInstance.GetCollection<PostEntity>(this.mongoDbOptions.PostsCollectionName)
                .Find(filter).Skip(page * limit).Limit(limit).ToListAsync();

            return new ServiceResult<IEnumerable<PostEntity>>(true, postEntities); 
        }

        public async Task<ServiceResult<bool>> UpdatePostAsync(PostEntity postEntity)
        {
            var validationErrors = postEntity.Validate();

            if (validationErrors.Any())
            {
                return new ServiceResult<bool>(false, validationErrors);
            }

            var isUpdated = await UpdatePostEntityAsync(postEntity);

            return new ServiceResult<bool>(isUpdated);
        }

        public async Task<ServiceResult<bool>> DeletePostAsync(string id)
        {
            var postEntity = await GetPostByIdAsync(id);
            if (postEntity == null)
            {
                return new ServiceResult<bool>(false);
            }
            else
            {
                postEntity.IsDeleted = true;
                var isDeleted = await UpdatePostEntityAsync(postEntity);

                return new ServiceResult<bool>(true);
            }
        }

        private async Task<PostEntity> GetPostByIdAsync(string id)
        {
            var postCollection = mongoDbInstance.GetCollection<PostEntity>(this.mongoDbOptions.PostsCollectionName);
            var postEntityCollection = await postCollection.FindAsync(Builders<PostEntity>.Filter.Eq("_id", ObjectId.Parse(id)));

            return await postEntityCollection.FirstOrDefaultAsync();
        }

        private async Task<bool> UpdatePostEntityAsync(PostEntity postEntity)
        {
            var postCollection = mongoDbInstance.GetCollection<PostEntity>(this.mongoDbOptions.PostsCollectionName);
            var replaceResult = await postCollection.ReplaceOneAsync(Builders<PostEntity>.Filter.Eq("_id", postEntity.Id), postEntity);

            return replaceResult.IsAcknowledged && replaceResult.MatchedCount > 0;
        }
    }
}
