using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using ExpressionTree;

namespace Test.NewtonsoftJson
{
    /// <summary>
    /// Expression converter for JSON.NET.
    /// </summary>
    public class ExpressionConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Expr);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jObject = JObject.Load(reader);
            Expr expr = new Expr();

            expr.Left = ReadValue(jObject["Left"]);
            expr.Operator = (OperatorEnum)Enum.Parse(typeof(OperatorEnum), (string)jObject["Operator"]);
            expr.Right = ReadValue(jObject["Right"]);

            return expr;
        }

        private object ReadValue(JToken token)
        {
            switch (token.Type)
            {
                case JTokenType.String:
                    string strValue = token.Value<string>();
                    // Try parsing as DateTime
                    if (DateTime.TryParse(strValue, out DateTime dateTimeValue))
                    {
                        return dateTimeValue;
                    }
                    return strValue;
                case JTokenType.Integer:
                    return token.Value<long>();
                case JTokenType.Float:
                    return token.Value<double>();
                case JTokenType.Boolean:
                    return token.Value<bool>();
                case JTokenType.Date:
                    return token.Value<DateTime>();
                case JTokenType.Null:
                    return null;
                case JTokenType.Object:
                    return token.ToObject<Expr>(new JsonSerializer { Converters = { new ExpressionConverter() } });
                case JTokenType.Array:
                    List<object> list = new List<object>();
                    foreach (var item in token.Children())
                    {
                        list.Add(ReadValue(item));
                    }
                    return list;
                default:
                    throw new JsonException($"Unexpected token type: {token.Type}");
            }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Expr expr = (Expr)value;
            writer.WriteStartObject();

            writer.WritePropertyName("Left");
            WriteValue(writer, expr.Left, serializer);

            writer.WritePropertyName("Operator");
            writer.WriteValue(expr.Operator.ToString());

            writer.WritePropertyName("Right");
            WriteValue(writer, expr.Right, serializer);

            writer.WriteEndObject();
        }

        private void WriteValue(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteNull();
            }
            else if (value is string str)
            {
                writer.WriteValue(str);
            }
            else if (value is long l)
            {
                writer.WriteValue(l);
            }
            else if (value is int i)
            {
                writer.WriteValue(i);
            }
            else if (value is double d)
            {
                writer.WriteValue(d);
            }
            else if (value is bool b)
            {
                writer.WriteValue(b);
            }
            else if (value is DateTime dt)
            {
                writer.WriteValue(dt);
            }
            else if (value is Expr expr)
            {
                serializer.Serialize(writer, expr);
            }
            else if (value is IEnumerable<object> list)
            {
                writer.WriteStartArray();
                foreach (var item in list)
                {
                    WriteValue(writer, item, serializer);
                }
                writer.WriteEndArray();
            }
            else
            {
                throw new JsonException($"Unexpected value type: {value.GetType()}");
            }
        }
    }
}
