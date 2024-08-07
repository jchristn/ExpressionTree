namespace Test.SystemTextJson
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Text.Json;
    using System.Text.Json.Serialization;
    using ExpressionTree;

    public class ExpressionConverter : JsonConverter<Expr>
    {
        // https://makolyte.com/system-text-json-how-to-customize-serialization-with-jsonconverter/
        // https://stackoverflow.com/questions/70539351/custom-converter-for-system-text-json-when-working-with-generics/70543894#70543894

        public override Expr Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            Expr ret = new Expr();

            bool inLeft = false;
            bool inOper = false;
            bool inRight = false;

            while (reader.Read())
            {
                if (!inLeft && !inOper && !inRight)
                {
                    if (reader.TokenType == JsonTokenType.PropertyName)
                    {
                        if (reader.ValueTextEquals("Left"))
                        {
                            inLeft = true;
                        }
                        else if (reader.ValueTextEquals("Operator"))
                        {
                            inOper = true;
                        }
                        else if (reader.ValueTextEquals("Right"))
                        {
                            inRight = true;
                        }
                    }
                }
                else
                {
                    if (inLeft)
                    { 
                        if (reader.TokenType == JsonTokenType.String)
                        {
                            ret.Left = reader.GetString();
                        }
                        else if (reader.TokenType == JsonTokenType.StartObject)
                        {
                            ret.Left = JsonSerializer.Deserialize<Expr>(ref reader, new JsonSerializerOptions
                            {
                                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                                Converters = { new JsonStringEnumConverter(), new ExpressionConverter() },
                            });
                        }
                        else if (reader.TokenType == JsonTokenType.StartArray)
                        {
                            ret.Left = JsonSerializer.Deserialize<List<object>>(ref reader, new JsonSerializerOptions
                            {
                                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                                Converters = { new JsonStringEnumConverter(), new ExpressionConverter() },
                            });
                        }
                        else if (reader.TokenType == JsonTokenType.Number)
                        {
                            // at some point in the future, it may be necessary to:
                            // 1) convert the decimal to a string
                            // 2) see if there is a decimal point
                            // 3) if there is, attach value as a decimal to ret
                            // 4) else attach as a long
                            ret.Left = reader.GetDecimal();
                        }
                        else if (reader.TokenType == JsonTokenType.True)
                        {
                            ret.Left = true;
                        }
                        else if (reader.TokenType == JsonTokenType.False)
                        {
                            ret.Left = false;
                        }
                        else
                        {
                            throw new InvalidOperationException("Unexpected token type '" + reader.TokenType.ToString() + "' while processing value for 'Left' expression.");
                        }
                    }
                    else if (inOper)
                    {
                        ret.Operator = (OperatorEnum)(Enum.Parse(typeof(OperatorEnum), reader.GetString()));
                    }
                    else if (inRight)
                    {
                        if (reader.TokenType == JsonTokenType.String)
                        {
                            ret.Right = reader.GetString();
                        }
                        else if (reader.TokenType == JsonTokenType.StartObject)
                        {
                            ret.Right = JsonSerializer.Deserialize<Expr>(ref reader, new JsonSerializerOptions
                            {
                                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                                Converters = { new JsonStringEnumConverter(), new ExpressionConverter() },
                            });
                        }
                        else if (reader.TokenType == JsonTokenType.StartArray)
                        {
                            ret.Right = JsonSerializer.Deserialize<List<object>>(ref reader, new JsonSerializerOptions
                            {
                                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                                Converters = { new JsonStringEnumConverter(), new ExpressionConverter() },
                            });
                        }
                        else if (reader.TokenType == JsonTokenType.Number)
                        {
                            // at some point in the future, it may be necessary to:
                            // 1) convert the decimal to a string
                            // 2) see if there is a decimal point
                            // 3) if there is, attach value as a decimal to ret
                            // 4) else attach as a long
                            ret.Right = reader.GetDecimal();
                        }
                        else if (reader.TokenType == JsonTokenType.True)
                        {
                            ret.Right = true;
                        }
                        else if (reader.TokenType == JsonTokenType.False)
                        {
                            ret.Right = false;
                        }
                        else
                        {
                            throw new InvalidOperationException("Unexpected token type '" + reader.TokenType.ToString() + "' while processing value for 'Left' expression.");
                        }
                    }                        

                    inLeft = false;
                    inOper = false;
                    inRight = false;
                }
            }

            return ret;
        }

        /// <summary>
        /// Write.
        /// </summary>
        /// <param name="writer">Writer.</param>
        /// <param name="value">Value.</param>
        /// <param name="options">Options.</param>
        public override void Write(Utf8JsonWriter writer, Expr value, JsonSerializerOptions options)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));

            writer.WriteStartObject();

            if (value.Left is Expr)
            {
                writer.WritePropertyName("Left");
                JsonSerializer.Serialize(writer, value.Left, options);
            }
            else if (IsNumberType(value.Left))
            {
                writer.WriteNumber("Left", Convert.ToDecimal(value.Left));
            }
            else if (IsBooleanType(value.Left))
            {
                writer.WriteBoolean("Left", Convert.ToBoolean(value.Left));
            }
            else
            {
                if (value.Left != null)
                {
                    writer.WriteString("Left", value.Left.ToString());
                }
                else
                {
                    writer.WriteNull("Left");
                }
            }

            writer.WriteString("Operator", value.Operator.ToString());

            if (value.Right is Expr)
            {
                writer.WritePropertyName("Right");
                JsonSerializer.Serialize(writer, value.Right, options);
            }
            else if (IsNumberType(value.Right))
            {
                writer.WriteNumber("Right", Convert.ToDecimal(value.Right));
            }
            else if (IsBooleanType(value.Right))
            {
                writer.WriteBoolean("Right", Convert.ToBoolean(value.Right));
            }
            else
            {
                if (value.Right != null)
                {
                    writer.WriteString("Right", value.Right.ToString());
                }
                else
                {
                    writer.WriteNull("Right");
                }
            }

            writer.WriteEndObject();
        }

        private bool IsNumberType(object value)
        {
            if (value == null) return false;
            return Double.TryParse(value.ToString(), out double _);
        }

        private bool IsBooleanType(object value)
        {
            if (value == null) return false;
            return Boolean.TryParse(value.ToString(), out bool _);
        }
    }
}
