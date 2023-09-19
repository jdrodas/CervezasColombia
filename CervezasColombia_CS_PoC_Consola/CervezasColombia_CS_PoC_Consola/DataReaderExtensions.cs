using System.Data;

namespace CervezasColombia_CS_PoC_Consola
{
    public static class DataReaderExtensions
    {
        public static IEnumerable<T> MapToList<T>(this IDataReader reader)
        {
            if (reader == null)
                throw new ArgumentNullException(nameof(reader));

            var results = new List<T>();
            var properties = typeof(T).GetProperties();

            while (reader.Read())
            {
                var obj = Activator.CreateInstance<T>();

                foreach (var property in properties)
                {
                    if (!reader.IsDBNull(reader.GetOrdinal(property.Name)))
                    {
                        var value = reader[property.Name];
                        property.SetValue(obj, value, null);
                    }
                }

                results.Add(obj);
            }

            return results;
        }
    }


}


