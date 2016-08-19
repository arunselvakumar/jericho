namespace MeMeSquad.Services.Interfaces
{
    using System.Collections.Generic;
    using Microsoft.Azure.Documents;
    using System.Threading.Tasks;

    public interface ITagService
    {
        Task CreateTagsAsync(IEnumerable<string> tags);

        Task<IEnumerable<Document>> GetPostsForTag(string tag);
    }
}