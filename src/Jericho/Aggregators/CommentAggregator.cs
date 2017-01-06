using Jericho.Aggregators.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Jericho.Models.v1.Entities;
using Jericho.Services.Interfaces;
using Jericho.Models.v1.BOs;

namespace Jericho.Aggregators
{
    public class CommentAggregator : ICommentAggregator
    {
        private readonly ICommentService commentService;

        public CommentAggregator(ICommentService commentService)
        {
            this.commentService = commentService;
        }

        public async Task AggregateCommentsForPost(PostBo postBo)
        {
            var postComments = await this.commentService.GetPostComments(postBo.Id);
            postBo.Comments = postComments;            
        }
    }
}
