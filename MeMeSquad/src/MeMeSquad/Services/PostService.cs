namespace MeMeSquad.Services
{
    using System;
    using MeMeSquad.Config;
    using Microsoft.Extensions.Options;
    using System.Threading.Tasks;
    using MeMeSquad.Services.Interfaces;
    using Microsoft.Azure.Documents;    
    using Microsoft.Azure.Documents.Client;
    using System.Collections.Generic;
    using System.Linq;
    using MeMeSquad.Models;

    public class PostService : IPostService
    {
        #region Fields

        private readonly DocumentDbConfig documentDbConfig;
        private IDocumentClient documentClient;
        #endregion

        #region Constructor

        public PostService(IOptions<DocumentDbConfig> documentDbConfig, IDocumentClient documentClient)
        {
            this.documentDbConfig = documentDbConfig.Value;
            this.documentClient = documentClient;

            this.InitializeDbConnection();
        }

        #endregion

        #region Public Methods

        public async Task CreatePostAsync(Post post, IEnumerable<string> tags)
        {
            await this.documentClient.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(this.documentDbConfig.DatabaseName, this.documentDbConfig.PostCollectionName), post);
        }

        public async Task<Document> GetPostAsync(string id)
        {
            var document = await this.documentClient.ReadDocumentAsync(UriFactory.CreateDocumentUri(this.documentDbConfig.DatabaseName, this.documentDbConfig.PostCollectionName, id));
            return document.Resource;
        }

        public IEnumerable<Post> GetAllPosts()
        {
            var documents = this.documentClient.CreateDocumentQuery<Post>(UriFactory.CreateDocumentCollectionUri(this.documentDbConfig.DatabaseName, this.documentDbConfig.PostCollectionName))
                .Where(document => document.IsActive)
                .OrderByDescending(document => document.Version);

            return documents;
        }
        #endregion

        #region Private Methods

        private void InitializeDbConnection()
        {
            // TechEd Europe https://www.youtube.com/watch?v=-5HKtWhqWC8
            // Enabled TCP & Direct Connection for increased throughput.
            var connectionPolicy = new ConnectionPolicy
            {
                ConnectionMode = ConnectionMode.Direct,
                ConnectionProtocol = Protocol.Tcp
            };

            this.documentClient = new DocumentClient(new Uri(this.documentDbConfig.EndPointUri), this.documentDbConfig.PrimaryKey, connectionPolicy);
        }
        #endregion
    }
}
