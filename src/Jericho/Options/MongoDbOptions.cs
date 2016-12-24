namespace Jericho.Options
{
    public sealed class MongoDbOptions
    {
        public string ConnectionString { get; set; }

        public string DatabaseName { get; set; }

        public string PostsCollectionName { get; set; }

        public string UsersCollectionName { get; set; }

        public string FavoritesCollectionName { get; set; }

        public string TagCollectionName { get; set; }

        public string CommentsCollectionName { get; set; }
    }
}