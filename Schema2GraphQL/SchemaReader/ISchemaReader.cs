using Schema2GraphQL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Schema2GraphQL.SchemaReader
{
    interface ISchemaReader
    {
        Task<IEnumerable<Schema>> ReadSchemasAsync(IEnumerable<string> collections);
    }
}
