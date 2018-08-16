using System.Collections.Generic;

namespace Schema2GraphQL.Models
{
    class Schema
    {
        public string Name { get; set; }

        public IDictionary<string, SchemaProperty> Properties { get; set; }
    }
}
