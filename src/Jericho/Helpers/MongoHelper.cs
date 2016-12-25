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
            MongoClient client = new MongoClient(this.mongoDbOptions.ConnectionString);
            return client.GetDatabase(this.mongoDbOptions.DatabaseName);
        }
    }
}
