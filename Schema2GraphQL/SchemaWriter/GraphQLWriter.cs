using System.IO;
using System.Threading.Tasks;

namespace Schema2GraphQL.SchemaWriter
{
    abstract class GraphQLWriter
    {
        protected ISchemaResolver Resolver { get; }

        protected GraphQLWriter(ISchemaResolver resolver)
        {
            Resolver = resolver;
        }

        protected async Task WriteToFileAsync(string filePath, string output)
        {
            var file = new FileInfo(filePath);

            using (var fs = file.Open(FileMode.OpenOrCreate))
            {
                fs.Seek(0, SeekOrigin.End);

                using (var writer = new StreamWriter(fs))
                {
                    await writer.WriteAsync(output);
                }
            }
        }
    }
}
