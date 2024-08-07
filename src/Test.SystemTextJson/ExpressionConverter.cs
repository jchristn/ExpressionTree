namespace Test.SystemTextJson
{
    using System;
    using System.Collections.Generic;
    using System.Text.Json;
    using System.Text.Json.Serialization;
    using ExpressionTree;

    /// <summary>
    /// Expression converter.
    /// </summary>
    public class ExpressionConverter : JsonConverter<Expr>
    {
        /// <summary>
        /// Read.
        /// </summary>
        /// <param name="reader">Reader.</param>
        /// <param name="typeToConvert">Type to convert.</param>
        /// <param name="options">Options.</param>
        /// <returns>Expr.</returns>
        public override Expr Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException("Expected start of object");
            }

            Expr expr = new Expr();

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                {
                    return expr;
                }

                if (reader.TokenType == JsonTokenType.PropertyName)
                {
                    string propertyName = reader.GetString();
                    reader.Read();

                    switch (propertyName)
                    {
                        case "Left":
                            expr.Left = ReadValue(ref reader);
                            break;
                        case "Operator":
                            expr.Operator = Enum.Parse<OperatorEnum>(reader.GetString());
                            break;
                        case "Right":
                            expr.Right = ReadValue(ref reader);
                            break;
                        default:
                            reader.Skip();
                            break;
                    }
                }
            }

            return expr;
        }

        private object ReadValue(ref Utf8JsonReader reader)
        {
            switch (reader.TokenType)
            {
                case JsonTokenType.String:
                    return reader.GetString();
                case JsonTokenType.Number:
                    if (reader.TryGetInt64(out long longValue))
                        return longValue;
                    return reader.GetDouble();
                case JsonTokenType.True:
                    return true;
                case JsonTokenType.False:
                    return false;
                case JsonTokenType.Null:
                    return null;
                case JsonTokenType.StartObject:
                    return JsonSerializer.Deserialize<Expr>(ref reader, new JsonSerializerOptions { Converters = { new ExpressionConverter() } });
                case JsonTokenType.StartArray:
                    List<object> list = new List<object>();
                    while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
                    {
                        list.Add(ReadValue(ref reader));
                    }
                    return list;
                default:
                    throw new JsonException($"Unexpected token type: {reader.TokenType}");
            }
        }

        /// <summary>
        /// Write.
        /// </summary>
        /// <param name="writer">Writer.</param>
        /// <param name="value">Value.</param>
        /// <param name="options">Options.</param>
        public override void Write(Utf8JsonWriter writer, Expr value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            writer.WritePropertyName("Left");
            WriteValue(writer, value.Left);

            writer.WritePropertyName("Operator");
            writer.WriteStringValue(value.Operator.ToString());

            writer.WritePropertyName("Right");
            WriteValue(writer, value.Right);

            writer.WriteEndObject();
        }

        private void WriteValue(Utf8JsonWriter writer, object value)
        {
            if (value == null)
            {
                writer.WriteNullValue();
            }
            else if (value is string str)
            {
                writer.WriteStringValue(str);
            }
            else if (value is long l)
            {
                writer.WriteNumberValue(l);
            }
            else if (value is int i)
            {
                writer.WriteNumberValue(i);
            }
            else if (value is double d)
            {
                writer.WriteNumberValue(d);
            }
            else if (value is bool b)
            {
                writer.WriteBooleanValue(b);
            }
            else if (value is Expr expr)
            {
                Write(writer, expr, null);
            }
            else if (value is IEnumerable<object> list)
            {
                writer.WriteStartArray();
                foreach (var item in list)
                {
                    WriteValue(writer, item);
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