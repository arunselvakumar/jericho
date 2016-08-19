namespace MeMeSquad.Config
{
    public class DocumentDbConfig
    {
        public string EndPointUri { get; set; }

        public string PrimaryKey { get; set; }

        public string DatabaseName { get; set; }

        public string PostCollectionName { get; set; }

        public string TagCollectionName { get; set; }
    }
}