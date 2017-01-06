namespace Jericho.Services.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Jericho.Providers.ServiceResultProvider;
    using Jericho.Models;
    using Jericho.Models.v1.Entities;

    using Microsoft.Azure.Documents;
    using Microsoft.AspNetCore.Http;
    using Models.v1.BOs;

    public interface ICommentService
    {
        Task<ServiceResult<CommentBo>> CreateCommentAsync(CommentBo commentBo);

        Task<ServiceResult<CommentBo>> GetCommentAsync(string id);

        Task<IEnumerable<CommentBo>> GetPostComments(string postId);
    }
}
