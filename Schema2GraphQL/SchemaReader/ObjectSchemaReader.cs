using Schema2GraphQL.ConnectionProviders;
using Schema2GraphQL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Schema2GraphQL.SchemaReader
{
    abstract class ObjectSchemaReader : ISchemaReader
    {
        protected INoSqlConnectionProvider Provider { get; }

        protected ObjectSchemaReader(INoSqlConnectionProvider provider)
        {
            Provider = provider;
        }

        public abstract Task<IEnumerable<Schema>> ReadSchemasAsync(IEnumerable<string> collections);
    }
}
