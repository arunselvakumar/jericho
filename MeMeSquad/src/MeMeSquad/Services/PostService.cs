namespace MeMeSquad.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;

    using Jericho.Helpers.Interfaces;

    using MeMeSquad.Config;
    using MeMeSquad.Models.v1.Entities;
    using MeMeSquad.Services.Interfaces;

    using Microsoft.Extensions.Options;

    using MongoDB.Driver;

    public class PostService : IPostService
    {
        #region Fields

        private readonly MongoDbConfig mongoDbConfig;
        private readonly IMongoDatabase mongoDbInstance;

        #endregion

        #region Constructor
        
        public PostService(IOptions<MongoDbConfig> MongoDbConfig, IMapper mapper, IMongoHelper mongoHelper)
        {
            this.mongoDbInstance = mongoHelper.MongoDbInstance;
            this.mongoDbConfig = MongoDbConfig.Value;
        }

        #endregion

        #region Public Methods

        public async Task<PostEntity> CreatePostAsync(PostEntity postEntity)
        {
            var postCollection = mongoDbInstance.GetCollection<PostEntity>(mongoDbConfig.PostsCollectionName);
            await postCollection.InsertOneAsync(postEntity);

            return await GetPostAsync(postEntity.Id.ToString());
        }

        public async Task<PostEntity> GetPostAsync(string id)
        {
            var postCollection = mongoDbInstance.GetCollection<PostEntity>(mongoDbConfig.PostsCollectionName);
            var postEntity = await postCollection.FindAsync(Builders<PostEntity>.Filter.Eq("_id", id));

            return await postEntity.FirstOrDefaultAsync();
        }

        public IEnumerable<PostEntity> GetAllPosts()
        {
            var postEntities = mongoDbInstance.GetCollection<PostEntity>(mongoDbConfig.PostsCollectionName)
                .AsQueryable()
                .Where(postEntity=>postEntity.IsActive)
                .OrderByDescending(postEntity=>postEntity.Version);

            return postEntities;
        }

        #endregion
      
    }
}
