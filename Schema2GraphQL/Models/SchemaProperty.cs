using System;

namespace Schema2GraphQL.Models
{
    class SchemaProperty
    {
        public string Name { get; set; }

        public bool IsKey { get; set; }

        public Type Type { get; set; }

        public bool IsRequired { get; set; }

        public bool IsUnique { get; set; }

        public object DefaultValue { get; set; }
    }
}
