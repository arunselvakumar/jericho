namespace MeMeSquad.Services
{
    using System;
    using System.Threading.Tasks;

    using MeMeSquad.Config;
    using MeMeSquad.Models.Entities;
    using MeMeSquad.Services.Interfaces;

    using Microsoft.Azure.Documents;
    using Microsoft.Azure.Documents.Client;
    using Microsoft.Extensions.Options;

    public class UserService : IUserService
    {
        #region Fields

        private readonly DocumentDbConfig documentDbConfig;

        private IDocumentClient documentClient;

        #endregion

        #region Constructor

        public UserService(IOptions<DocumentDbConfig> documentDbConfig)
        {
            this.documentDbConfig = documentDbConfig.Value;

            this.InitializeDbConnection();
        }

        #endregion

        #region Public Methods

        public async Task CreateUserAsync(UserEntity user)
        {
            var documentUri = UriFactory.CreateDocumentCollectionUri(this.documentDbConfig.DatabaseName, this.documentDbConfig.UsersCollectionName);
            await this.documentClient.CreateDocumentAsync(documentUri, user);
        }

        public Task LoginUserAsync(UserEntity user)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> IsUserNameExistsAsync(string username)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsEmailAddressExistsAsync(string email)
        {
            throw new NotImplementedException();
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