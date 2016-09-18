namespace MeMeSquad.Services
{
    using System;
    using System.Linq;
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

        public bool LoginUserAsync(UserEntity user)
        {
            var isAnyDocumentsFound = this.documentClient.CreateDocumentQuery<UserEntity>(UriFactory.CreateDocumentCollectionUri(this.documentDbConfig.DatabaseName, this.documentDbConfig.UsersCollectionName))
                .AsEnumerable()
                .Any(document => this.IsUserAuthorized(user, document));

            return isAnyDocumentsFound;
        }

        public bool IsUserNameExists(string username)
        {
            var isAnyDocumentsFound = this.documentClient.CreateDocumentQuery<UserEntity>(UriFactory.CreateDocumentCollectionUri(this.documentDbConfig.DatabaseName, this.documentDbConfig.UsersCollectionName))
                .AsEnumerable()
                .Any(document => document.UserName.Equals(username.Trim()));

            return isAnyDocumentsFound;
        }

        public bool IsEmailAddressExists(string email)
        {
            var isAnyDocumentsFound = this.documentClient.CreateDocumentQuery<UserEntity>(UriFactory.CreateDocumentCollectionUri(this.documentDbConfig.DatabaseName, this.documentDbConfig.UsersCollectionName))
                .AsEnumerable()
                .Any(document => document.EMail.Equals(email.Trim()));

            return isAnyDocumentsFound;
        }

        #endregion

        #region Private Methods

        private bool IsUserAuthorized(UserEntity user, UserEntity document)
        {
            var isUserNameAndPasswordMatches = user.UserName.Equals(document.UserName) && user.Password.Equals(document.Password);
            var isEmailAddressAndPasswordMathes = user.UserName.Equals(document.EMail) && user.Password.Equals(document.Password);

            return isUserNameAndPasswordMatches || isEmailAddressAndPasswordMathes;
        }

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