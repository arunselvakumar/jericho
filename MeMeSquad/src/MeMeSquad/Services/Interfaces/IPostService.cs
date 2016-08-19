namespace MeMeSquad.Services.Interfaces
{
    using Entity;
    using Microsoft.Azure.Documents;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IPostService
    {
        Task CreatePostAsync(Post post, IEnumerable<string> tags);

        Task<Document> GetPostAsync(string id);

        Task<IEnumerable<Document>> GetPostsForTag(string tag);
        
        IEnumerable<Post> GetAllPosts();
    }
}
