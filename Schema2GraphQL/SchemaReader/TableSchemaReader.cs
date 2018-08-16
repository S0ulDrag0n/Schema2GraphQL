using Schema2GraphQL.ConnectionProviders;
using Schema2GraphQL.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace Schema2GraphQL.SchemaReader
{
    class TableSchemaReader : ISchemaReader
    {
        protected ISqlConnectionProvider Provider { get; }

        public TableSchemaReader(ISqlConnectionProvider provider)
        {
            Provider = provider;
        }

        public async Task<IEnumerable<Schema>> ReadSchemasAsync(IEnumerable<string> collections)
        {
            var restrictions = new string[4];

            if (collections.Contains("*"))
            {
                using (var connection = Provider.Provide())
                {
                    await connection.OpenAsync();

                    restrictions[0] = connection.Database;

                    var dbTables = connection.GetSchema("Tables", restrictions);
                    var tableNames = new List<string>();

                    foreach (DataRow row in dbTables.Rows)
                    {
                        var tableName = row[2].ToString();
                        tableNames.Add(tableName);
                    }

                    collections = tableNames;
                }
            }

            var schemas = new List<Schema>(collections.Count());

            foreach (var collection in collections)
            {
                var schema = new Schema
                {
                    Name = collection,
                    Properties = new Dictionary<string, SchemaProperty>()
                };

                using (var table = new DataTable())
                {
                    using (var connection = Provider.Provide())
                    {
                        connection.Open();

                        var factory = DbProviderFactories.GetFactory(connection);

                        using (var command = factory.CreateCommand())
                        {
                            command.Connection = connection;
                            command.CommandText = $"SELECT TOP 1 * FROM [{collection}]";

                            using (var adapter = factory.CreateDataAdapter())
                            {
                                adapter.SelectCommand = command;
                                adapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;

                                try
                                {
                                    adapter.FillSchema(table, SchemaType.Source);
                                }
                                catch
                                {
                                    continue;
                                }
                            }
                        }
                    }

                    foreach (DataColumn column in table.Columns)
                    {
                        schema.Properties.Add(column.ColumnName, new SchemaProperty
                        {
                            Name = column.ColumnName,
                            IsKey = table.PrimaryKey.Contains(column),
                            Type = column.DataType,
                            DefaultValue = column.DefaultValue,
                            IsRequired = column.AllowDBNull,
                            IsUnique = column.Unique
                        });
                    }
                }

                schemas.Add(schema);
            }

            return schemas;
        }
    }
}
