namespace Jericho.Services.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Jericho.Providers.ServiceResultProvider;
    using Jericho.Models;
    using Jericho.Models.v1.Entities;

    using Microsoft.Azure.Documents;
    using Microsoft.AspNetCore.Http;

    public interface ICommentService
    {
        Task<ServiceResult<CommentEntity>> CreateCommentAsync(CommentEntity post);

        Task<CommentEntity> GetCommentAsync(string id);

        Task<IEnumerable<CommentEntity>> GetPostComments(string postId);
    }
}
