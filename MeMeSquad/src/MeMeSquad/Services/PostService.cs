namespace MeMeSquad.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;

    using MeMeSquad.Config;
    using MeMeSquad.Models.Entities;
    using MeMeSquad.Services.Interfaces;

    using Microsoft.Azure.Documents;
    using Microsoft.Azure.Documents.Client;
    using Microsoft.Extensions.Options;

    public class PostService : IPostService
    {
        #region Fields

        private readonly DocumentDbConfig documentDbConfig;
        private IDocumentClient documentClient;

        #endregion

        #region Constructor

        public PostService(IOptions<DocumentDbConfig> documentDbConfig, IMapper mapper)
        {
            this.documentDbConfig = documentDbConfig.Value;

            this.InitializeDbConnection();
        }

        #endregion

        #region Public Methods

        public async Task CreatePostAsync(PostEntity post, IEnumerable<string> tags)
        {
            var documentUri = UriFactory.CreateDocumentCollectionUri(this.documentDbConfig.DatabaseName, this.documentDbConfig.PostsCollectionName);
            await this.documentClient.CreateDocumentAsync(documentUri, post);
        }

        public async Task<PostEntity> GetPostAsync(string id)
        {
            var documentUri = UriFactory.CreateDocumentUri(this.documentDbConfig.DatabaseName, this.documentDbConfig.PostsCollectionName, id);
            var document = await this.documentClient.ReadDocumentAsync(documentUri);
            return (dynamic)document.Resource;
        }

        public IEnumerable<PostEntity> GetAllPosts()
        {
            var documents = this.documentClient.CreateDocumentQuery<PostEntity>(UriFactory.CreateDocumentCollectionUri(this.documentDbConfig.DatabaseName, this.documentDbConfig.PostsCollectionName))
                .AsEnumerable()
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
