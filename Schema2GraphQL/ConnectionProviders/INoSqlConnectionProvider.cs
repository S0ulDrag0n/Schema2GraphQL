namespace Schema2GraphQL.ConnectionProviders
{
    interface INoSqlConnectionProvider
    {
        dynamic Provide();
    }
}
