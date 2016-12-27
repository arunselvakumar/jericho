namespace Jericho.Helpers
{
    using Jericho.Helpers.Interfaces;
    using Jericho.Options;

    using Microsoft.Extensions.Options;

    using MongoDB.Driver;
    using System.Security.Authentication;

    /// <summary>
    /// 
    /// </summary>
    public class MongoHelper : IMongoHelper
    {
        private readonly IMongoDatabase mongoDBInstance;
        private readonly MongoDbOptions mongoDbOptions;

        /// <summary>
        /// 
        /// </summary>
        public IMongoDatabase MongoDbInstance
        {
            get
            {
                return mongoDBInstance;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mongoDbConfig"></param>
        public MongoHelper(IOptions<MongoDbOptions> mongoDbConfig)
        {
            this.mongoDbOptions = mongoDbConfig.Value;
            this.mongoDBInstance = CreateMongoDbInstance();
        }


        private IMongoDatabase CreateMongoDbInstance()
        {
            MongoClientSettings settings = MongoClientSettings.FromUrl(new MongoUrl(this.mongoDbOptions.ConnectionString));
            settings.SslSettings = new SslSettings() { EnabledSslProtocols = SslProtocols.Tls12 };
            MongoClient client = new MongoClient(settings);

            return client.GetDatabase(this.mongoDbOptions.DatabaseName);
        }
    }
}
