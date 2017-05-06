namespace Jericho.Providers
{
    using System.Security.Authentication;

    using Jericho.Options;
    using Jericho.Providers.Interfaces;

    using Microsoft.Extensions.Options;

    using MongoDB.Driver;

    public class DataProvider : IDataProvider
    {
        public DataProvider(IOptions<MongoDbOptions> mongoDbOptions)
        {
            this.ParameterCollections = mongoDbOptions.Value;

            this.InitializeDatabaseConnection();
        }

        public MongoDbOptions ParameterCollections { get; }

        public IMongoDatabase Connection { get; private set; }

        public void Refresh()
        {
            this.InitializeDatabaseConnection();
        }

        private void InitializeDatabaseConnection()
        {
            var settings = MongoClientSettings.FromUrl(new MongoUrl(this.ParameterCollections.ConnectionString));
            settings.SslSettings = new SslSettings() { EnabledSslProtocols = SslProtocols.Tls12 };
            var mongoClient = new MongoClient(settings);

            this.Connection = mongoClient.GetDatabase(this.ParameterCollections.DatabaseName);
        }
    }
}
