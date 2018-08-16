using System.Data.Common;
using System.Data.SqlClient;

namespace Schema2GraphQL.ConnectionProviders
{
    class SqlServerConnectionProvider : ConnectionProvider<SqlConnectionStringBuilder, DbConnection>, ISqlConnectionProvider
    {
        protected SqlConnectionStringBuilder Builder { get; }

        public SqlServerConnectionProvider(ConnectionInfo info) : base(info)
        {
            Builder = new SqlConnectionStringBuilder(Info.ConnectionString);
        }

        public override DbConnection Create()
        {
            var url = Builder.ToString();

            var connection = new SqlConnection(url);

            return connection;
        }

        public DbConnection Provide()
        {
            return Create();
        }
    }
}
