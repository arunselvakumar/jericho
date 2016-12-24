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

    public class CommentService : ICommentService
    {
        private readonly MongoDbOptions mongoDbOptions;
        private readonly IMongoDatabase mongoDbInstance;

        public CommentService(IOptions<MongoDbOptions> MongoDbConfig, IMongoHelper mongoHelper)
        {
            this.mongoDbInstance = mongoHelper.MongoDbInstance;
            this.mongoDbOptions = MongoDbConfig.Value;
        }

        public async Task<ServiceResult<CommentEntity>> CreateCommentAsync(CommentEntity commentEntity)
        {   
            Console.WriteLine("entered");
            var validationErrors = commentEntity.Validate();

            if (validationErrors.Any())
            {
                return new ServiceResult<CommentEntity>(false, validationErrors);
            }

            commentEntity.ApplyPresets();
            var commentCollection = mongoDbInstance.GetCollection<CommentEntity>(this.mongoDbOptions.CommentsCollectionName);
            await commentCollection.InsertOneAsync(commentEntity);

            var insertedEntity = await GetCommentAsync(commentEntity.Id.ToString());

            return new ServiceResult<CommentEntity>(true, insertedEntity);
        }

        public async Task<CommentEntity> GetCommentAsync(string id)
        {
            var commentCollection = mongoDbInstance.GetCollection<CommentEntity>(this.mongoDbOptions.CommentsCollectionName);
            var commentEntity = await commentCollection.FindAsync(Builders<CommentEntity>.Filter.Eq("_id", ObjectId.Parse(id)));

            return await commentEntity.FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<CommentEntity>> GetPostComments(string postId)
        {
            var commentCollection = mongoDbInstance.GetCollection<CommentEntity>(this.mongoDbOptions.CommentsCollectionName);
            return await commentCollection.Find(Builders<CommentEntity>.Filter.Eq("postid", ObjectId.Parse(postId))).ToListAsync();
        }
    }
}
