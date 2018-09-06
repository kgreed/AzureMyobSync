using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace SBD.AMS.MYOB
{
	public static class JsonExtensions
	{
        public static string ToJson2(this object entity)
        {
            var serializer = new JsonSerializer();
            serializer.Converters.Add(new StringEnumConverter());
            using (var writer = new StringWriter())
            {
                serializer.Serialize(writer, entity);
                return writer.ToString();
            }
        }
    }
}