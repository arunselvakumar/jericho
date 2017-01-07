namespace Jericho.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Jericho.Helpers.Interfaces;
    using Jericho.Models.v1.Entities;
    using Jericho.Options;
    using Jericho.Services.Interfaces;
    using Jericho.Providers.ServiceResultProvider;

    using Microsoft.Extensions.Options;

    using MongoDB.Driver;
    using MongoDB.Bson;
    using Models.v1.Entities.Extensions;
    using AutoMapper;
    using Models.v1.BOs;

    public class CommentService : ICommentService
    {
        private readonly MongoDbOptions mongoDbOptions;
        private readonly IMongoDatabase mongoDbInstance;
        private readonly IMapper mapper;

        public CommentService(IOptions<MongoDbOptions> MongoDbConfig, IMongoHelper mongoHelper, IMapper mapper)
        {
            this.mongoDbInstance = mongoHelper.MongoDbInstance;
            this.mongoDbOptions = MongoDbConfig.Value;
            this.mapper = mapper;
        }

        public async Task<ServiceResult<CommentBo>> CreateCommentAsync(CommentBo commentBo)
        {
            var commentEntity = this.mapper.Map<CommentEntity>(commentBo);
            var validationErrors = commentEntity.Validate();

            if (validationErrors.Any())
            {
                return new ServiceResult<CommentBo>(false, validationErrors);
            }

            commentEntity.ApplyPresets();
            var commentCollection = mongoDbInstance.GetCollection<CommentEntity>(this.mongoDbOptions.CommentsCollectionName);
            await commentCollection.InsertOneAsync(commentEntity);

            var insertedEntity = await GetCommentByIdAsync(commentEntity.Id.ToString());
            var insertedBo = this.mapper.Map<CommentBo>(insertedEntity);

            return new ServiceResult<CommentBo>(true, insertedBo);
        }

        private async Task<CommentEntity> GetCommentByIdAsync(string id)
        {
            var commentCollection = mongoDbInstance.GetCollection<CommentEntity>(this.mongoDbOptions.CommentsCollectionName);
            var commentEntities = await commentCollection.FindAsync(Builders<CommentEntity>.Filter.Eq("_id", ObjectId.Parse(id)));

            return await commentEntities.FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<CommentBo>> GetPostComments(string postId)
        {
            var commentCollection = mongoDbInstance.GetCollection<CommentEntity>(this.mongoDbOptions.CommentsCollectionName);
            var commentEntities = await commentCollection.Find(Builders<CommentEntity>.Filter.Eq("postid", postId)).ToListAsync();

            return this.mapper.Map<IEnumerable<CommentBo>>(commentEntities);
        }
    }
}
