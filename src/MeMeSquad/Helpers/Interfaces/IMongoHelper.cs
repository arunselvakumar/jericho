using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Jericho.Helpers.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface IMongoHelper
    {
        /// <summary>
        /// 
        /// </summary>
        IMongoDatabase MongoDbInstance { get; }
    }
}
