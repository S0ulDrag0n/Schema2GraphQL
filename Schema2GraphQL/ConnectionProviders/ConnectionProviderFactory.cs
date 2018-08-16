namespace Schema2GraphQL.ConnectionProviders
{
    static class ConnectionProviderFactory
    {
        public static dynamic Create(ConnectionInfo info)
        {
            if (info.ConnectionString.StartsWith("mongodb://"))
            {
                return new MongoConnectionProvider(info);
            }

            return new SqlServerConnectionProvider(info);
        }
    }
}
