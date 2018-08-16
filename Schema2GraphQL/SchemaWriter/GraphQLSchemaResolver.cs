using Humanizer;
using Schema2GraphQL.Models;
using System;
using System.Linq;

namespace Schema2GraphQL.SchemaWriter
{
    class GraphQLSchemaResolver : ISchemaResolver
    {
        protected const string RequiredSymbol = "!";
        protected const string UniqueKeyword = "@unique";

        public string ResolveModelName(Schema schema)
        {
            return schema.Name;
        }

        public string ResolveAsParameterName(string name)
        {
            return name.Singularize(inputIsKnownToBePlural: false).Camelize();
        }

        public string ResolveAsArrayParameterName(string name)
        {
            return ResolveAsParameterName(name).Pluralize();
        }

        public string ResolveSchemaPropertyType(Type t)
        {
            if (t == typeof(bool) || t == typeof(byte))
            {
                return "Boolean";
            }

            if (t.Name.Contains("int", StringComparison.OrdinalIgnoreCase))
            {
                return "Int";
            }

            if (t == typeof(float) || t == typeof(double) || t == typeof(decimal))
            {
                return "Float";
            }

            return "String";
        }

        public string ResolveMutationPropertyName(MutationType opType, Schema schema)
        {
            switch (opType)
            {
                case MutationType.Update:
                    return $"update{schema.Name.Pascalize()}";
                case MutationType.Delete:
                    return $"delete{schema.Name.Pascalize()}";
                default:
                    return $"create{schema.Name.Pascalize()}";
            }
        }

        public string ResolveSchemaAsParameter(Schema schema, bool required = true)
        {
            return $"{ResolveAsParameterName(schema.Name)}: {ResolveModelName(schema)}Input{(required ? RequiredSymbol : string.Empty)}";
        }

        public string ResolvePropertyAsParameter(SchemaProperty property)
        {
            return $"{ResolveAsParameterName(property.Name)}: {ResolveSchemaPropertyType(property.Type)}{(property.IsRequired || property.IsKey ? RequiredSymbol : string.Empty)}";
        }

        public string ResolveQueryManyProperty(Schema schema, bool allowEmpty = true, bool nullable = false)
        {
            return $"{ResolveAsArrayParameterName(schema.Name)}: [{ResolveModelName(schema)}{(allowEmpty ? RequiredSymbol : string.Empty)}]{(nullable ? string.Empty : RequiredSymbol)}";
        }

        public string ResolveQueryOneProperty(Schema schema, bool allowEmpty = true, bool nullable = false)
        {
            var keys = schema.Properties.Values.Where(p => p.IsKey).ToList();

            if (keys.Any())
            {
                var parameterizedKeys = string.Join(", ", keys.Select(pk => ResolvePropertyAsParameter(pk)));

                return $"{ResolveAsParameterName(schema.Name)}({parameterizedKeys}): {ResolveModelName(schema)}{RequiredSymbol}";
            }

            return $"{ResolveAsParameterName(schema.Name)}({ResolveSchemaAsParameter(schema)}): {ResolveModelName(schema)}{RequiredSymbol}";
        }

        public string ResolveCreateMutationProperty(Schema schema)
        {
            return $"{ResolveMutationPropertyName(MutationType.Create, schema)}({ResolveSchemaAsParameter(schema)}): {ResolveModelName(schema)}{RequiredSymbol}";
        }

        public string ResolveUpdateMutationProperty(Schema schema)
        {
            return $"{ResolveMutationPropertyName(MutationType.Update, schema)}({ResolveSchemaAsParameter(schema)}): {ResolveModelName(schema)}{RequiredSymbol}";
        }

        public string ResolveDeleteMutationProperty(Schema schema)
        {
            var keys = schema.Properties.Values.Where(p => p.IsKey).ToList();

            if (keys.Any())
            {
                var parameterizedKeys = string.Join(", ", keys.Select(pk => ResolvePropertyAsParameter(pk)));

                return $"{ResolveMutationPropertyName(MutationType.Delete, schema)}({parameterizedKeys}): {ResolveModelName(schema)}{RequiredSymbol}";
            }

            return $"{ResolveMutationPropertyName(MutationType.Delete, schema)}({ResolveSchemaAsParameter(schema)}): {ResolveModelName(schema)}{RequiredSymbol}";
        }

        public string ResolveSchemaIdProperty(SchemaProperty property)
        {
            return $"{property.Name}: ID! {UniqueKeyword}";
        }

        public string ResolveSchemaProperty(SchemaProperty property)
        {
            var resolvedType = ResolveSchemaPropertyType(property.Type);

            var required = property.IsRequired ? RequiredSymbol : string.Empty;
            var defaultValue = property.DefaultValue != null && !string.IsNullOrEmpty(property.DefaultValue.ToString()) ? $" default({property.DefaultValue})" : string.Empty;
            var unique = property.IsUnique ? $" {UniqueKeyword}" : string.Empty;

            return $"{property.Name}: {resolvedType}{required}{defaultValue}{unique}";
        }

        public string ResolveQueryManyResolver(Schema schema)
        {
            return $"{ResolveAsArrayParameterName(schema.Name)}: () => {{ throw 'Not Implemented' }},";
        }

        public string ResolveQueryOneResolver(Schema schema)
        {
            var keys = schema.Properties.Values.Where(p => p.IsKey).ToList();

            if (keys.Any())
            {
                var parameterizedKeys = string.Join(", ", keys.Select(pk => ResolveAsParameterName(pk.Name)));
                var parameters = keys.Count > 1 ? $"({parameterizedKeys})" : parameterizedKeys;

                return $"{ResolveAsParameterName(schema.Name)}: {parameters} => {{ throw 'Not Implemented' }},";
            }

            return $"{ResolveAsParameterName(schema.Name)}: {ResolveAsParameterName(schema.Name)} => {{ throw 'Not Implemented' }},";
        }

        public string ResolveCreateMutationResolver(Schema schema)
        {
            return $"{ResolveMutationPropertyName(MutationType.Create, schema)}: {ResolveAsParameterName(schema.Name)} => {{ throw 'Not Implemented' }},";
        }

        public string ResolveUpdateMutationResolver(Schema schema)
        {
            return $"{ResolveMutationPropertyName(MutationType.Update, schema)}: {ResolveAsParameterName(schema.Name)} => {{ throw 'Not Implemented' }},";
        }

        public string ResolveDeleteMutationResolver(Schema schema)
        {
            var keys = schema.Properties.Values.Where(p => p.IsKey).ToList();

            if (keys.Any())
            {
                var parameterizedKeys = string.Join(", ", keys.Select(pk => ResolveAsParameterName(pk.Name)));
                var parameters = keys.Count > 1 ? $"({parameterizedKeys})" : parameterizedKeys;

                return $"{ResolveMutationPropertyName(MutationType.Delete, schema)}: {parameters} => {{ throw 'Not Implemented' }},";
            }

            return $"{ResolveMutationPropertyName(MutationType.Delete, schema)}: {ResolveAsParameterName(schema.Name)} => {{ throw 'Not Implemented' }},";
        }
    }
}
