namespace MeMeSquad.Config
{
    public class DocumentDbConfig
    {
        public string EndPointUri { get; set; }

        public string PrimaryKey { get; set; }

        public string DatabaseName { get; set; }

        public string PostsCollectionName { get; set; }

        public string UsersCollectionName { get; set; }

        public string TagCollectionName { get; set; }
    }
}