using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Test.NewtonsoftJson
{
    internal static class SerializationHelper
    {
        internal static string SerializeJson(object obj, bool pretty)
        {
            if (obj == null) return null;
            string json;

            if (pretty)
            {
                json = JsonConvert.SerializeObject(
                  obj,
                  Newtonsoft.Json.Formatting.Indented,
                  new JsonSerializerSettings
                  {
                      Converters = DefaultConverters,
                      NullValueHandling = NullValueHandling.Ignore,
                      DateTimeZoneHandling = DateTimeZoneHandling.Utc
                  });
            }
            else
            {
                json = JsonConvert.SerializeObject(obj,
                  new JsonSerializerSettings
                  {
                      Converters = DefaultConverters,
                      NullValueHandling = NullValueHandling.Ignore,
                      DateTimeZoneHandling = DateTimeZoneHandling.Utc
                  });
            }

            return json;
        }
        
        internal static JsonConverter[] DefaultConverters = { new ExpressionConverter() };

        internal static T DeserializeJson<T>(string json)
        {
            if (String.IsNullOrEmpty(json)) throw new ArgumentNullException(nameof(json));
            return JsonConvert.DeserializeObject<T>(json, 
                new JsonSerializerSettings
                {
                    Converters = DefaultConverters,
                    TypeNameHandling = TypeNameHandling.All
                });
        }
    }
}
