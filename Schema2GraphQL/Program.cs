using Schema2GraphQL.ConnectionProviders;
using Schema2GraphQL.SchemaReader;
using Schema2GraphQL.SchemaWriter;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Schema2GraphQL
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var connectionInfo = new ConnectionInfo();
            var collections = new [] { "*" };
            var outputPath = "./output";
            var resolverFile = "resolver.js";
            var separateFiles = false;

            for (int i = 0; i < args.Length; i++)
            {
                string arg = args[i];
                if (arg.Equals("-c", StringComparison.OrdinalIgnoreCase))
                {
                    connectionInfo.ConnectionString = args[++i];
                }
                else if (arg.Equals("-t", StringComparison.OrdinalIgnoreCase))
                {
                    collections = args[++i].Split(",");
                }
                else if (arg.Equals("-o", StringComparison.OrdinalIgnoreCase))
                {
                    outputPath = args[++i];
                }
                else if (arg.Equals("-s", StringComparison.OrdinalIgnoreCase))
                {
                    separateFiles = true;
                }
                else if (arg.Equals("-r", StringComparison.OrdinalIgnoreCase))
                {
                    resolverFile = args[++i];
                }
            }

            if (!string.IsNullOrEmpty(connectionInfo.ConnectionString))
            {
                var connectionProvider = ConnectionProviderFactory.Create(connectionInfo);

                var graphTypeResolver = new GraphQLSchemaResolver();

                var reader = new TableSchemaReader(connectionProvider);
                var schemaWriter = new GraphQLSchemaWriter(graphTypeResolver);
                var resolverWriter = new GraphQLResolverWriter(graphTypeResolver);

                Console.WriteLine("Reading schemas...");

                var schemas = await reader.ReadSchemasAsync(collections);

                Directory.CreateDirectory(outputPath);

                var queryFile = separateFiles ? "_query" : "schema";
                var mutationFile = separateFiles ? "_mutation" : "schema";

                await schemaWriter.WriteQueryTypeAsync($"{outputPath}{Path.DirectorySeparatorChar}{queryFile}.graphql", schemas);

                await schemaWriter.WriteMutationTypeAsync($"{outputPath}{Path.DirectorySeparatorChar}{mutationFile}.graphql", schemas);

                foreach (var schema in schemas)
                {
                    Directory.CreateDirectory(outputPath);

                    Console.WriteLine($"Writing schema for {schema.Name}");

                    await schemaWriter.WriteModelTypeAsync($"{outputPath}{Path.DirectorySeparatorChar}{(separateFiles ? schema.Name : "schema")}.graphql", schema);

                    await schemaWriter.WriteInputTypeAsync($"{outputPath}{Path.DirectorySeparatorChar}{(separateFiles ? schema.Name : "schema")}.graphql", schema);
                }

                Console.WriteLine($"Writing resolver...");

                await resolverWriter.WriteResolverFileAsync($"{outputPath}{Path.DirectorySeparatorChar}{resolverFile}", schemas);
            }

            Console.WriteLine("Complete! Press enter to exit.");

            Console.ReadLine();
        }
    }
}
