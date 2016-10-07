namespace Jericho.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;
    using Jericho.Helpers.Interfaces;
    using Jericho.Models.v1.Entities;
    using Jericho.Options;
    using Jericho.Services.Interfaces;

    using Microsoft.Extensions.Options;

    using MongoDB.Driver;
    using Models.v1.Entities.Enums;
    using MongoDB.Bson;
    using Microsoft.AspNetCore.Http;
    using Models.v1.Entities.Extensions;
    using Extensions;

    public class PostService : IPostService
    {
        #region Fields

        private readonly MongoDbOptions mongoDbOptions;
        private readonly IMongoDatabase mongoDbInstance;

        #endregion

        #region Constructor
        
        public PostService(IOptions<MongoDbOptions> MongoDbConfig, IMapper mapper, IMongoHelper mongoHelper)
        {
            this.mongoDbInstance = mongoHelper.MongoDbInstance;
            this.mongoDbOptions = MongoDbConfig.Value;
        }

        #endregion

        #region Public Methods

        public async Task<PostEntity> CreatePostAsync(PostEntity postEntity)
        {
            postEntity.ApplyPresets();
            var postCollection = mongoDbInstance.GetCollection<PostEntity>(this.mongoDbOptions.PostsCollectionName);
            await postCollection.InsertOneAsync(postEntity);

            return await GetPostAsync(postEntity.Id.ToString());
        }

        public async Task<PostEntity> GetPostAsync(string id)
        {
            var postCollection = mongoDbInstance.GetCollection<PostEntity>(this.mongoDbOptions.PostsCollectionName);            
            var postEntity = await postCollection.FindAsync(Builders<PostEntity>.Filter.Eq("_id", ObjectId.Parse(id)));

            return await postEntity.FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<PostEntity>> GetPosts(IQueryCollection query, int page, int limit)
        {            
            var filter = new BsonDocument(query.ToDictionary(kvp => kvp.Key.ToLower(), kvp => kvp.Value[0]));
            filter.RemoveDefaultPostFilterPresets();
            filter.ApplyDefaultPostFilterPresets();

            return await mongoDbInstance.GetCollection<PostEntity>(this.mongoDbOptions.PostsCollectionName)
                .Find(filter).Skip(page*limit).Limit(limit).ToListAsync();
        }

        public async Task<bool> UpdatePostAsync(PostEntity postEntity)
        {
            var postCollection = mongoDbInstance.GetCollection<PostEntity>(this.mongoDbOptions.PostsCollectionName);
            var replaceResult = await postCollection.ReplaceOneAsync(Builders<PostEntity>.Filter.Eq("_id", postEntity.Id), postEntity);
            return replaceResult.IsAcknowledged && replaceResult.MatchedCount > 0;
        }

        public async Task<bool> DeletePostAsync(string id)
        {
            var postEntity = await GetPostAsync(id);
            if(postEntity == null)
            {
                return false;
            }
            else
            {
                postEntity.IsDeleted = true;
                return await UpdatePostAsync(postEntity);
            }            
        }

        #endregion

    }
}
