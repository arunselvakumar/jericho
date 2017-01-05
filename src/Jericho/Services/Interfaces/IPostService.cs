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

    public interface IPostService
    {
        Task<ServiceResult<UpdatePostBo>> CreatePostAsync(PostEntity post);

        Task<ServiceResult<UpdatePostBo>> GetPostAsync(string id);
        
        Task<ServiceResult<IEnumerable<UpdatePostBo>>> GetPostsAsync(IQueryCollection query, int page, int limit);

        Task<ServiceResult<bool>> UpdatePostAsync(PostEntity postEntity);

        Task<ServiceResult<bool>> DeletePostAsync(string id);
        
    }
}
