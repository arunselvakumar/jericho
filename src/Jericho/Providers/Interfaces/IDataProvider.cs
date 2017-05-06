namespace Jericho.Providers.Interfaces
{
    using Jericho.Options;

    using MongoDB.Driver;

    public interface IDataProvider
    {
        IMongoDatabase Connection { get; }

        MongoDbOptions ParameterCollections { get; }

        void Refresh();
    }
}
