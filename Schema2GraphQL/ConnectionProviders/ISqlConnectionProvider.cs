using System.Data.Common;

namespace Schema2GraphQL.ConnectionProviders
{
    interface ISqlConnectionProvider
    {
        DbConnection Provide();
    }
}
