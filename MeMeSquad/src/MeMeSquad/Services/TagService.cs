using Microsoft.Azure.Documents;

namespace MeMeSquad.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using MeMeSquad.Services.Interfaces;

    public class TagService : ITagService
    {
        public Task CreateTagsAsync(IEnumerable<string> tags)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<Document>> GetPostsForTag(string tag)
        {
            throw new System.NotImplementedException();
        }
    }
}