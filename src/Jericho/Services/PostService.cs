namespace Jericho.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Aggregators.Interfaces;

    using AutoMapper;

    using Extensions;

    using Jericho.Models.v1.Entities;
    using Jericho.Providers.Interfaces;
    using Jericho.Providers;
    using Jericho.Services.Interfaces;

    using Microsoft.AspNetCore.Http;

    using Models.v1.BOs;

    using MongoDB.Bson;
    using MongoDB.Driver;

    public class PostService : IPostService
    {
        private readonly IDataProvider dataProvider;
        private readonly ICommentAggregator commentAggregator;
        private readonly IMapper mapper;

        public PostService(IDataProvider dataProvider, IMapper mapper, ICommentAggregator commentAggregator)
        {
            this.dataProvider = dataProvider;
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

            var postCollection = this.dataProvider.Connection.GetCollection<PostEntity>(this.dataProvider.ParameterCollections.PostsCollectionName);
            await postCollection.InsertOneAsync(postEntity);

            var insertedEntity = await this.GetPostByIdAsync(postEntity.Id.ToString());
            var insertedBo = this.mapper.Map<PostBo>(insertedEntity);           

            return new ServiceResult<PostBo>(true, insertedBo);
        }

        public async Task<ServiceResult<PostBo>> GetPostAsync(string id)
        {
            var postEntity = await this.GetPostByIdAsync(id);

            if (postEntity == null)
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

            var postEntities = await this.dataProvider.Connection.GetCollection<PostEntity>(this.dataProvider.ParameterCollections.PostsCollectionName)
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

            var isUpdated = await this.UpdatePostEntityAsync(postEntity);

            return new ServiceResult<bool>(isUpdated);
        }

        public async Task<ServiceResult<bool>> DeletePostAsync(string id)
        {
            var postEntity = await this.GetPostByIdAsync(id);
            if (postEntity == null)
            {
                return new ServiceResult<bool>(false);
            }
            else
            {
                postEntity.IsDeleted = true;
                var isDeleted = await this.UpdatePostEntityAsync(postEntity);

                return new ServiceResult<bool>(true);
            }
        }

        private async Task<PostEntity> GetPostByIdAsync(string id)
        {
            var postCollection = this.dataProvider.Connection.GetCollection<PostEntity>(this.dataProvider.ParameterCollections.PostsCollectionName);
            var postEntityCollection = await postCollection.FindAsync(Builders<PostEntity>.Filter.Eq("_id", ObjectId.Parse(id)));

            return await postEntityCollection.FirstOrDefaultAsync();
        }

        private async Task<bool> UpdatePostEntityAsync(PostEntity postEntity)
        {
            var postCollection = this.dataProvider.Connection.GetCollection<PostEntity>(this.dataProvider.ParameterCollections.PostsCollectionName);
            var replaceResult = await postCollection.ReplaceOneAsync(Builders<PostEntity>.Filter.Eq("_id", postEntity.Id), postEntity);

            return replaceResult.IsAcknowledged && replaceResult.MatchedCount > 0;
        }
    }
}
