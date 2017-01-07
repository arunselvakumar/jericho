namespace Jericho.Services.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Jericho.Providers.ServiceResultProvider;
    using Microsoft.AspNetCore.Http;
    using Models.v1.BOs;

    public interface IPostService
    {
        Task<ServiceResult<PostBo>> CreatePostAsync(PostBo postBo);

        Task<ServiceResult<PostBo>> GetPostAsync(string id);
        
        Task<ServiceResult<IEnumerable<PostBo>>> GetPostsAsync(IQueryCollection query, int page, int limit);

        Task<ServiceResult<bool>> UpdatePostAsync(PostBo postEntity);

        Task<ServiceResult<bool>> DeletePostAsync(string id);
        
    }
}
