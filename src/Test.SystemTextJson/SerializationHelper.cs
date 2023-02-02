using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace Test.SystemTextJson
{
    /// <summary>
    /// Serialization helper.
    /// </summary>
    internal static class SerializationHelper
    {
        #region Internal-Members

        #endregion

        #region Private-Members

        private static JsonSerializerOptions _OptionsNotPretty = new JsonSerializerOptions
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            Converters = { new JsonStringEnumConverter() },
        };

        private static JsonSerializerOptions _OptionsPretty = new JsonSerializerOptions
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            Converters = { new JsonStringEnumConverter() },
            WriteIndented = true
        };

        private static JsonSerializerOptions _OptionsDeserializer = new JsonSerializerOptions
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            Converters = { new JsonStringEnumConverter(), new ExpressionConverter() },
        };

        #endregion

        #region Internal-Methods

        /// <summary>
        /// Serialize an object to JSON.
        /// </summary>
        /// <typeparam name="T">Type.</typeparam>
        /// <param name="obj">Object.</param>
        /// <param name="pretty">Flag indicating whether or not pretty printing should be used.</param>
        /// <returns>JSON string.</returns>
        internal static string SerializeJson<T>(T obj, bool pretty = false)
        {
            return JsonSerializer.Serialize(
                obj,
                typeof(T),
                (pretty ? _OptionsPretty : _OptionsNotPretty));
        }

        /// <summary>
        /// Deserialize JSON to an object.
        /// </summary>
        /// <typeparam name="T">Type.</typeparam>
        /// <param name="json">JSON string.</param>
        /// <returns>Instance of T.</returns>
        internal static T DeserializeJson<T>(string json)
        {
            return JsonSerializer.Deserialize<T>(json, _OptionsDeserializer);
        }

        #endregion

        #region Private-Methods

        #endregion
    }
}
