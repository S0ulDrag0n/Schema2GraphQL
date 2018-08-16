namespace Schema2GraphQL.ConnectionProviders
{
    abstract class ConnectionProvider<TConnectionBuilder, TConnection>
    {
        public ConnectionInfo Info { get; set; }

        protected ConnectionProvider(ConnectionInfo info)
        {
            Info = info;
        }

        public abstract TConnection Create();
    }
}
