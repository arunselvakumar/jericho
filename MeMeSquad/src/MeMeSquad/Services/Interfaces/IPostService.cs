namespace MeMeSquad.Services.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using MeMeSquad.Models;
    using MeMeSquad.Models.Entities;

    using Microsoft.Azure.Documents;

    public interface IPostService
    {
        Task CreatePostAsync(PostEntity post, IEnumerable<string> tags);

        Task<PostEntity> GetPostAsync(string id);
        
        IEnumerable<PostEntity> GetAllPosts();
    }
}
