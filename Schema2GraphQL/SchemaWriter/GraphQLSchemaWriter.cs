using Schema2GraphQL.Models;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Schema2GraphQL.SchemaWriter
{
    class GraphQLSchemaWriter : GraphQLWriter
    {
        public GraphQLSchemaWriter(ISchemaResolver resolver) : base(resolver)
        {
        }

        public async Task WriteQueryTypeAsync(string filePath, IEnumerable<Schema> schemas)
        {
            var builder = new StringBuilder();

            builder.AppendLine("type Query {");

            foreach (var schema in schemas)
            {
                builder.AppendLine($"\t{Resolver.ResolveQueryManyProperty(schema)}");

                builder.AppendLine($"\t{Resolver.ResolveQueryOneProperty(schema)}");
            }

            builder.AppendLine("}").AppendLine();

            var output = builder.ToString();

            await WriteToFileAsync(filePath, output);
        }

        public async Task WriteMutationTypeAsync(string filePath, IEnumerable<Schema> schemas)
        {
            var builder = new StringBuilder();

            builder.AppendLine("type Mutation {");

            foreach (var schema in schemas)
            {
                builder.AppendLine($"\t{Resolver.ResolveCreateMutationProperty(schema)}");

                builder.AppendLine($"\t{Resolver.ResolveUpdateMutationProperty(schema)}");

                builder.AppendLine($"\t{Resolver.ResolveDeleteMutationProperty(schema)}");
            }

            builder.AppendLine("}").AppendLine();

            var output = builder.ToString();

            await WriteToFileAsync(filePath, output);
        }

        public async Task WriteModelTypeAsync(string filePath, Schema schema)
        {
            var builder = new StringBuilder();

            builder.AppendLine($"type {Resolver.ResolveModelName(schema)} {{");

            foreach (var kv in schema.Properties)
            {
                var property = kv.Value;

                if (property.IsKey)
                {
                    builder.AppendLine($"\t{Resolver.ResolveSchemaIdProperty(property)}");
                }
                else
                {
                    builder.AppendLine($"\t{Resolver.ResolveSchemaProperty(property)}");
                }
            }

            builder.AppendLine("}").AppendLine();

            var output = builder.ToString();

            await WriteToFileAsync(filePath, output);
        }

        public async Task WriteInputTypeAsync(string filePath, Schema schema)
        {
            var builder = new StringBuilder();

            builder.AppendLine($"input {Resolver.ResolveModelName(schema)}Input {{");

            foreach (var kv in schema.Properties)
            {
                var property = kv.Value;

                if (property.IsKey)
                {
                    builder.AppendLine($"\t{Resolver.ResolveSchemaIdProperty(property)}");
                }
                else
                {
                    builder.AppendLine($"\t{Resolver.ResolveSchemaProperty(property)}");
                }
            }

            builder.AppendLine("}").AppendLine();

            var output = builder.ToString();

            await WriteToFileAsync(filePath, output);
        }
    }
}
