namespace MeMeSquad.Config
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class MongoDbConfig
    {
        /// <summary>
        /// 
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string DatabaseName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string PostsCollectionName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string UsersCollectionName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string TagCollectionName { get; set; }
    }
}