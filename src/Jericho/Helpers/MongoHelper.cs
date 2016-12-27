namespace Jericho.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Jericho.Configuration;
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

            //string connectionString =   @"mongodb://jericho-dev:D61z5cf4ZPndJBIejojCaUxPWnnZIjGrefZpvUkD5lGlHfQY7ONz1HWoZHBYbQPdEJdLyP8yL324hJWnCxuPAg==@jericho-dev.documents.azure.com:10250/?ssl=true&sslverifycertificate=false";
            MongoClientSettings settings = MongoClientSettings.FromUrl(
              new MongoUrl(this.mongoDbOptions.ConnectionString)
            );
            settings.SslSettings =
              new SslSettings() { EnabledSslProtocols = SslProtocols.Tls12 };


            //MongoClient client = new MongoClient(this.mongoDbOptions.ConnectionString);
            MongoClient client = new MongoClient(settings);

            return client.GetDatabase(this.mongoDbOptions.DatabaseName);
        }
    }
}
