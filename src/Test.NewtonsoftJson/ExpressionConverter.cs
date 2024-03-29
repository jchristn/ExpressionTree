﻿using System;
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
            return (objectType == typeof(Expr));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            // see https://blog.codeinside.eu/2015/03/30/json-dotnet-deserialize-to-abstract-class-or-interface/

            Expr ret = new Expr();

            JObject jo = JObject.Load(reader);

            /*
            Console.WriteLine("--- In Converter ---");
            Console.WriteLine("Object   : " + jo.ToString());
            Console.WriteLine("Left     : " + jo["Left"].ToString());
            Console.WriteLine("Operator : " + jo["Operator"].ToString());
            Console.WriteLine("Right    : " + jo["Right"].ToString());
            */

            if (jo["Left"] != null)
            {
                JToken leftToken = jo["Left"];
                if (leftToken.Type == JTokenType.Object)
                {
                    // Console.WriteLine("Left: Object");
                    ret.Left = SerializationHelper.DeserializeJson<Expr>(leftToken.ToString());
                }
                else if (leftToken.Type == JTokenType.Array)
                {
                    // Console.WriteLine("Left: Array");
                    ret.Left = leftToken.ToObject<List<object>>();
                }
                else if (leftToken.Type == JTokenType.Integer)
                {
                    // Console.WriteLine("Left: Integer");
                    ret.Left = leftToken.ToObject<decimal>();
                }
                else
                {
                    // Console.WriteLine("Left: Array");
                    ret.Left = leftToken.ToObject<string>();
                }
            }

            ret.Operator = (OperatorEnum)(Enum.Parse(typeof(OperatorEnum), jo["Operator"].ToString()));

            if (jo["Right"] != null)
            {
                JToken rightToken = jo["Right"];
                if (rightToken.Type == JTokenType.Object)
                {
                    // Console.WriteLine("Right: Object");
                    ret.Right = SerializationHelper.DeserializeJson<Expr>(rightToken.ToString());
                }
                else if (rightToken.Type == JTokenType.Array)
                {
                    // Console.WriteLine("Right: Array");
                    ret.Right = rightToken.ToObject<List<object>>();
                }
                else if (rightToken.Type == JTokenType.Integer)
                {
                    // Console.WriteLine("Right: Integer");
                    ret.Right = rightToken.ToObject<decimal>();
                }
                else
                {
                    // Console.WriteLine("Right: String");
                    ret.Right = rightToken.ToObject<string>();
                }
            }

            return ret;
        }

        public override bool CanWrite
        {
            get { return false; }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
