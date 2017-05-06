namespace Jericho.Aggregators
{
    using System.Threading.Tasks;

    using Jericho.Aggregators.Interfaces;
    using Jericho.Models.v1.BOs;
    using Jericho.Services.Interfaces;

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
