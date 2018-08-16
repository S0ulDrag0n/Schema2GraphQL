using System;

namespace Schema2GraphQL.SchemaReader
{
    interface ISqlTypeResolver
    {
        Type Resolve(string t);
    }
}
