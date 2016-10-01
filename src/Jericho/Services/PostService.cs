namespace Jericho.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;

    using Jericho.Config;
    using Jericho.Helpers.Interfaces;
    using Jericho.Models.v1.Entities;
    using Jericho.Options;
    using Jericho.Services.Interfaces;

    using Microsoft.Extensions.Options;

    using MongoDB.Driver;
    using Models.v1.Entities.Enums;

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
            var postCollection = mongoDbInstance.GetCollection<PostEntity>(this.mongoDbOptions.PostsCollectionName);
            await postCollection.InsertOneAsync(postEntity);

            return await GetPostAsync(postEntity.Id.ToString());
        }

        public async Task<PostEntity> GetPostAsync(string id)
        {
            var postCollection = mongoDbInstance.GetCollection<PostEntity>(this.mongoDbOptions.PostsCollectionName);
            var postEntity = await postCollection.FindAsync(Builders<PostEntity>.Filter.Eq("_id", id));

            return await postEntity.FirstOrDefaultAsync();
        }

        public IEnumerable<PostEntity> GetAllPosts()
        {
            var postEntities = mongoDbInstance.GetCollection<PostEntity>(this.mongoDbOptions.PostsCollectionName)
                .AsQueryable()
                .Where(postEntity=>postEntity.Status == PostStatusEnum.Approved && !postEntity.IsDeleted)                
                .OrderByDescending(postEntity=>postEntity.Version);

            return postEntities;
        }

        #endregion
      
    }
}
