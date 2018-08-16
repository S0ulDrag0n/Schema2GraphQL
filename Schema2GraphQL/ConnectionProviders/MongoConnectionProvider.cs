using MongoDB.Driver;

namespace Schema2GraphQL.ConnectionProviders
{
    class MongoConnectionProvider : ConnectionProvider<MongoUrlBuilder, IMongoClient>, INoSqlConnectionProvider
    {
        private MongoUrlBuilder Builder { get; }

        public MongoConnectionProvider(ConnectionInfo info) : base(info)
        {
           Builder = new MongoUrlBuilder(Info.ConnectionString);
        }

        public override IMongoClient Create()
        {
            var url = Builder.ToMongoUrl();

            var client = new MongoClient(url);

            return client;
        }

        public dynamic Provide()
        {
            var client = Create();

            var database = client.GetDatabase(Builder.DatabaseName);

            return database;
        }
    }
}
