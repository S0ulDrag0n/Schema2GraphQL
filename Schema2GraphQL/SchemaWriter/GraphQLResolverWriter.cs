using Schema2GraphQL.Models;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Schema2GraphQL.SchemaWriter
{
    class GraphQLResolverWriter : GraphQLWriter
    {
        public GraphQLResolverWriter(ISchemaResolver resolver) : base(resolver)
        {
        }

        public async Task WriteResolverFileAsync(string filePath, IEnumerable<Schema> schemas)
        {
            var builder = new StringBuilder();

            builder.AppendLine("{");

            builder.AppendLine("\tQuery: {");

            foreach (var schema in schemas)
            {
                builder.AppendLine($"\t\t{Resolver.ResolveQueryManyResolver(schema)}");

                builder.AppendLine($"\t\t{Resolver.ResolveQueryOneResolver(schema)}");
            }

            builder.AppendLine("\t},");

            builder.AppendLine("\tMutation: {");

            foreach (var schema in schemas)
            {
                builder.AppendLine($"\t\t{Resolver.ResolveCreateMutationResolver(schema)}");

                builder.AppendLine($"\t\t{Resolver.ResolveUpdateMutationResolver(schema)}");

                builder.AppendLine($"\t\t{Resolver.ResolveDeleteMutationResolver(schema)}");
            }

            builder.AppendLine("\t}");

            builder.AppendLine("}").AppendLine();

            var output = builder.ToString();

            await WriteToFileAsync(filePath, output);
        }
    }
}
