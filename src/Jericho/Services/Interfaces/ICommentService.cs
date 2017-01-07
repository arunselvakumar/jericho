namespace Jericho.Services.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Jericho.Providers.ServiceResultProvider;
    using Models.v1.BOs;

    public interface ICommentService
    {
        Task<ServiceResult<CommentBo>> CreateCommentAsync(CommentBo commentBo);

        Task<IEnumerable<CommentBo>> GetPostComments(string postId);
    }
}
