using Jericho.Helpers.Interfaces;
using MeMeSquad.Config;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Jericho.Helpers
{
    /// <summary>
    /// 
    /// </summary>
    public class MongoHelper : IMongoHelper
    {
        private readonly IMongoDatabase mongoDBInstance;
        private readonly MongoDbConfig mongoDbConfig;

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
        public MongoHelper(IOptions<MongoDbConfig> mongoDbConfig)
        {
            this.mongoDbConfig = mongoDbConfig.Value;
            this.mongoDBInstance = CreateMongoDbInstance();
        }


        private IMongoDatabase CreateMongoDbInstance()
        {
            MongoClient client = new MongoClient(mongoDbConfig.ConnectionString);
            return client.GetDatabase(mongoDbConfig.DatabaseName);
        }
    }
}
