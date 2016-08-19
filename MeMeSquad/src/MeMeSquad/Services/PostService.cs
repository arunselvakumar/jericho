using System;

namespace MeMeSquad.Services
{
    using System.Threading.Tasks;
    using MeMeSquad.Services.Interfaces;
    using Microsoft.Azure.Documents;
    using Entity;
    using Microsoft.Azure.Documents.Client;
    using System.Collections.Generic;
    using System.Linq;

    public class PostService : IPostService
    {
        #region Fields
        private readonly ITagService tagService;
        private IDocumentClient documentClient;
        #endregion

        #region Constructor

        public PostService(IDocumentClient documentClient, ITagService tagService)
        {
            this.documentClient = documentClient;
            this.tagService = tagService;

            this.InitializeProperties();
        }

        #endregion

        #region Public Methods

        public async Task CreatePostAsync(Post post, IEnumerable<string> tags)
        {
            await this.tagService.CreateTagsAsync(tags);
            await this.documentClient.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri("DBNAME", "COLLECTIONNAME"), post);
        }

        public async Task<Document> GetPostAsync(string id)
        {
            var document = await this.documentClient.ReadDocumentAsync(UriFactory.CreateDocumentUri("DBNAME", "COLLECTIONNAME", id));
            return document.Resource;
        }

        public Task<IEnumerable<Document>> GetPostsForTag(string tag)
        {
            return this.tagService.GetPostsForTag(tag);
        }

        public IEnumerable<Post> GetAllPosts()
        {
            var documents = this.documentClient.CreateDocumentQuery<Post>(UriFactory.CreateDocumentCollectionUri("DBNAME", "COLLECTIONNAME"))
                .Where(document => document.IsActive)
                .OrderByDescending(document => document.Version);

            return documents;
        }
        #endregion

        #region Private Methods

        private void InitializeProperties()
        {
            // TechEd Europe https://www.youtube.com/watch?v=-5HKtWhqWC8
            // Enabled TCP & Direct Connection for increased throughput.
            var connectionPolicy = new ConnectionPolicy
            {
                ConnectionMode = ConnectionMode.Direct,
                ConnectionProtocol = Protocol.Tcp
            };

            this.documentClient = new DocumentClient(new Uri("URI"), string.Empty, connectionPolicy);
        }
        #endregion
    }
}
