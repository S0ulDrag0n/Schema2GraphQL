using System;

namespace Schema2GraphQL.SchemaReader
{
    class TableColumnTypeResolver : ISqlTypeResolver
    {
        public Type Resolve(string t)
        {
            if (t.Contains("bit", StringComparison.OrdinalIgnoreCase)
                || t.Contains("binary", StringComparison.OrdinalIgnoreCase))
            {
                return typeof(bool);
            }

            if (t.Contains("int", StringComparison.OrdinalIgnoreCase))
            {
                return typeof(int);
            }

            if (t.Equals("float", StringComparison.OrdinalIgnoreCase)
                || t.Equals("real", StringComparison.OrdinalIgnoreCase)
                || t.Equals("double", StringComparison.OrdinalIgnoreCase)
                || t.Equals("numeric", StringComparison.OrdinalIgnoreCase)
                || t.Equals("decimal", StringComparison.OrdinalIgnoreCase)
                || t.Contains("money", StringComparison.OrdinalIgnoreCase))
            {
                return typeof(decimal);
            }

            if (t.Contains("date", StringComparison.OrdinalIgnoreCase)
                || t.Contains("time", StringComparison.OrdinalIgnoreCase)
                || t.Contains("year", StringComparison.OrdinalIgnoreCase))
            {
                return typeof(DateTime);
            }

            return typeof(string);
        }
    }
}
