using Schema2GraphQL.Models;
using System;

namespace Schema2GraphQL.SchemaWriter
{
    enum MutationType
    {
        Create,
        Update,
        Delete
    }

    interface ISchemaResolver
    {
        string ResolveModelName(Schema schema);

        string ResolveAsParameterName(string name);

        string ResolveAsArrayParameterName(string name);

        string ResolveSchemaPropertyType(Type t);

        string ResolveMutationPropertyName(MutationType opType, Schema schema);

        string ResolveSchemaAsParameter(Schema schema, bool required = true);

        string ResolvePropertyAsParameter(SchemaProperty property);

        string ResolveQueryManyProperty(Schema schema, bool allowEmpty = true, bool nullable = false);

        string ResolveQueryOneProperty(Schema schema, bool allowEmpty = true, bool nullable = false);

        string ResolveCreateMutationProperty(Schema schema);

        string ResolveUpdateMutationProperty(Schema schema);

        string ResolveDeleteMutationProperty(Schema schema);

        string ResolveSchemaIdProperty(SchemaProperty property);

        string ResolveSchemaProperty(SchemaProperty property);

        string ResolveQueryManyResolver(Schema schema);

        string ResolveQueryOneResolver(Schema schema);

        string ResolveCreateMutationResolver(Schema schema);

        string ResolveUpdateMutationResolver(Schema schema);

        string ResolveDeleteMutationResolver(Schema schema);
    }
}
