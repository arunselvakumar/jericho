namespace MeMeSquad.Services.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using MeMeSquad.Models;
    using MeMeSquad.Models.v1.Entities;

    using Microsoft.Azure.Documents;

    public interface IPostService
    {
        Task<PostEntity> CreatePostAsync(PostEntity post);

        Task<PostEntity> GetPostAsync(string id);
        
        IEnumerable<PostEntity> GetAllPosts();
    }
}
