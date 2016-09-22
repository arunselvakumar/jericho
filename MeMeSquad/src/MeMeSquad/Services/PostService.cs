namespace MeMeSquad.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;

    using MeMeSquad.Config;
    using MeMeSquad.Models.Entities;
    using MeMeSquad.Services.Interfaces;

    using Microsoft.Azure.Documents;
    using Microsoft.Azure.Documents.Client;
    using Microsoft.Extensions.Options;
    using Jericho.Helpers.Interfaces;
    using MongoDB.Driver;

    public class PostService : IPostService
    {
        #region Fields

        private readonly MongoDbConfig mongoDbConfig;
        private readonly IMongoHelper mongoHelper;

        #endregion

        #region Constructor
        
        public PostService(IOptions<MongoDbConfig> MongoDbConfig, IMapper mapper, IMongoHelper mongoHelper)
        {
            this.mongoHelper = mongoHelper;
            this.mongoDbConfig = MongoDbConfig.Value;
        }

        #endregion

        #region Public Methods

        public async Task CreatePostAsync(PostEntity post, IEnumerable<string> tags)
        {
            //var documentUri = UriFactory.CreateDocumentCollectionUri(this.MongoDbConfig.DatabaseName, this.MongoDbConfig.PostsCollectionName);
            //await this.documentClient.CreateDocumentAsync(documentUri, post);
        }

        public async Task<PostEntity> GetPostAsync(string id)
        {
            var postCollection = mongoHelper.MongoDbInstance.GetCollection<PostEntity>(mongoDbConfig.PostsCollectionName);
            var postEntity = await postCollection.FindAsync(entity => entity.Id == id);

            return await postEntity.FirstOrDefaultAsync();
        }

        public IEnumerable<PostEntity> GetAllPosts()
        {
            var postEntities = mongoHelper.MongoDbInstance.GetCollection<PostEntity>(mongoDbConfig.PostsCollectionName)
                .AsQueryable()
                .Where(postEntity=>postEntity.IsActive)
                .OrderByDescending(postEntity=>postEntity.Version);

            return postEntities;
        }

        #endregion
      
    }
}
