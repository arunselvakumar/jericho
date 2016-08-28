using System.Net;
using AutoMapper;
using MeMeSquad.Models.DTOs;

namespace MeMeSquad.Services
{
    using MeMeSquad.Models.Entities;
    using Microsoft.Extensions.Logging;
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
        private readonly IMapper mapper;
        private IDocumentClient documentClient;

        #endregion

        #region Constructor

        public PostService(IOptions<DocumentDbConfig> documentDbConfig, IMapper mapper)
        {
            this.documentDbConfig = documentDbConfig.Value;
            this.mapper = mapper;

            this.InitializeDbConnection();
        }

        #endregion

        #region Public Methods

        public async Task CreatePostAsync(PostEntity post, IEnumerable<string> tags)
        {
            await this.documentClient.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(this.documentDbConfig.DatabaseName, this.documentDbConfig.PostCollectionName), post);
        }

        public async Task<Document> GetPostAsync(string id)
        {
            var document = await this.documentClient.ReadDocumentAsync(UriFactory.CreateDocumentUri(this.documentDbConfig.DatabaseName, this.documentDbConfig.PostCollectionName, id));
            return document.Resource;
        }

        public IEnumerable<PostEntity> GetAllPosts()
        {
            var documents = this.documentClient.CreateDocumentQuery<PostEntity>(UriFactory.CreateDocumentCollectionUri(this.documentDbConfig.DatabaseName, this.documentDbConfig.PostCollectionName))
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
