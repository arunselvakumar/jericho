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
    using Aggregators.Interfaces;
    using AutoMapper;
    using Models.v1.BOs;

    public class PostService : IPostService
    {
        private readonly MongoDbOptions mongoDbOptions;
        private readonly IMongoDatabase mongoDbInstance;
        private readonly ICommentAggregator commentAggregator;
        private readonly IMapper mapper;

        public PostService(IOptions<MongoDbOptions> MongoDbConfig, IMongoHelper mongoHelper, IMapper mapper, ICommentAggregator commentAggregator)
        {
            this.mongoDbInstance = mongoHelper.MongoDbInstance;
            this.mongoDbOptions = MongoDbConfig.Value;
            this.mapper = mapper;
            this.commentAggregator = commentAggregator;
        }

        public async Task<ServiceResult<PostBo>> CreatePostAsync(PostBo postBo)
        {
            var postEntity = this.mapper.Map<PostEntity>(postBo);
            var validationErrors = postEntity.Validate();

            if (validationErrors.Any())
            {
                return new ServiceResult<PostBo>(false, validationErrors);
            }

            var postCollection = mongoDbInstance.GetCollection<PostEntity>(this.mongoDbOptions.PostsCollectionName);
            await postCollection.InsertOneAsync(postEntity);

            var insertedEntity = await GetPostByIdAsync(postEntity.Id.ToString());
            var insertedBo = this.mapper.Map<PostBo>(insertedEntity);           

            return new ServiceResult<PostBo>(true, insertedBo);
        }

        public async Task<ServiceResult<PostBo>> GetPostAsync(string id)
        {
            var postEntity = await this.GetPostByIdAsync(id);

            if(postEntity == null)
            {
                return new ServiceResult<PostBo>(false);
            }

            var postBo = this.mapper.Map<PostBo>(postEntity);
            await this.commentAggregator.AggregateCommentsForPost(postBo);

            return new ServiceResult<PostBo>(true, postBo);
        }

        public async Task<ServiceResult<IEnumerable<PostBo>>> GetPostsAsync(IQueryCollection query, int page, int limit)
        {
            var filter = new BsonDocument(query.ToDictionary(kvp => kvp.Key.ToLower(), kvp => kvp.Value[0].ToString().ToCaseInsensitiveRegex()));
            filter.RemoveDefaultPostFilterPresets();
            filter.ApplyDefaultPostFilterPresets();

            var postEntities = await mongoDbInstance.GetCollection<PostEntity>(this.mongoDbOptions.PostsCollectionName)
                .Find(filter).Skip(page * limit).Limit(limit).ToListAsync();

            var postBos = this.mapper.Map<IEnumerable<PostBo>>(postEntities);

            return new ServiceResult<IEnumerable<PostBo>>(true, postBos); 
        }

        public async Task<ServiceResult<bool>> UpdatePostAsync(PostBo postBo)
        {
            var postEntity = this.mapper.Map<PostEntity>(postBo);
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
