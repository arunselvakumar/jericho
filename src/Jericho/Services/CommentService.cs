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
    using System;
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

        public async Task<ServiceResult<CommentBo>> CreateCommentAsync(CommentEntity commentEntity)
        {   
            Console.WriteLine("entered");
            var validationErrors = commentEntity.Validate();

            if (validationErrors.Any())
            {
                return new ServiceResult<CommentBo>(false, validationErrors);
            }

            commentEntity.ApplyPresets();
            var commentCollection = mongoDbInstance.GetCollection<CommentEntity>(this.mongoDbOptions.CommentsCollectionName);
            await commentCollection.InsertOneAsync(commentEntity);

            var insertedEntity = await GetCommentAsync(commentEntity.Id.ToString());
            var insertedBo = this.mapper.Map<CommentBo>(insertedEntity);

            return new ServiceResult<CommentBo>(true, insertedBo);
        }

        public async Task<ServiceResult<CommentBo>> GetCommentAsync(string id)
        {
            var commentCollection = mongoDbInstance.GetCollection<CommentEntity>(this.mongoDbOptions.CommentsCollectionName);
            var commentEntities = await commentCollection.FindAsync(Builders<CommentEntity>.Filter.Eq("_id", ObjectId.Parse(id)));

            var commentEntity = await commentEntities.FirstOrDefaultAsync();

            if(commentEntity == null)
            {
                return new ServiceResult<CommentBo>(false);
            }

            var commentBo = this.mapper.Map<CommentBo>(commentEntity);

            return new ServiceResult<CommentBo>(true, commentBo);
        }

        public async Task<IEnumerable<CommentBo>> GetPostComments(string postId)
        {
            var commentCollection = mongoDbInstance.GetCollection<CommentEntity>(this.mongoDbOptions.CommentsCollectionName);
            var commentEntities = await commentCollection.Find(Builders<CommentEntity>.Filter.Eq("postid", ObjectId.Parse(postId))).ToListAsync();

            return this.mapper.Map<IEnumerable<CommentBo>>(commentEntities);
        }
    }
}
