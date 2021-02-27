using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TIMLJSON.Classes
{
    class KeyframeConverter : JsonConverter<Keyframe>
    {

        public override bool CanConvert(Type typeToConvert) =>
            typeof(Keyframe).IsAssignableFrom(typeToConvert);

        public override Keyframe Read(
            ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException();
            }

            reader.Read();
            if (reader.TokenType != JsonTokenType.PropertyName)
            {
                throw new JsonException();
            }

            string propertyName = reader.GetString();
            if (propertyName != "keyframeType")
            {
                throw new JsonException();
            }

            reader.Read();
            if (reader.TokenType != JsonTokenType.Number)
            {
                throw new JsonException();
            }

            int keyframeType = reader.GetInt32();
            Keyframe keyframe = keyframeType switch
            {
                1 => new IntKey(),
                2 => new FloatKey(),
                3 => new ColorKey(),
                _ => throw new JsonException()
            };
            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                {
                    return keyframe;
                }
                if (reader.TokenType == JsonTokenType.PropertyName)
                {
                    propertyName = reader.GetString();
                    reader.Read();
                    switch (propertyName)
                    {
                        case "MainData":
                            if (keyframe is FloatKey floatKey)
                            {
                                reader.Read();
                                if (reader.TokenType == JsonTokenType.PropertyName)
                                {
                                    reader.Read();
                                    floatKey.MainData = new FloatObj(reader.GetSingle());
                                    reader.Read();
                                }
                            }
                            if (keyframe is IntKey intKey)
                            {
                                reader.Read();
                                if (reader.TokenType == JsonTokenType.PropertyName)
                                {
                                    reader.Read();
                                    intKey.MainData = new IntObj(reader.GetInt32());
                                    reader.Read();
                                }
                            }
                            if (keyframe is ColorKey colorKey)
                            {
                                reader.Read();
                                if (reader.TokenType == JsonTokenType.PropertyName)
                                {
                                    reader.Read();
                                    colorKey.MainData.red = reader.GetByte();
                                    reader.Read();
                                    reader.Read();
                                    colorKey.MainData.green = reader.GetByte();
                                    reader.Read();
                                    reader.Read();
                                    colorKey.MainData.blue = reader.GetByte();
                                    reader.Read();
                                    reader.Read();
                                    colorKey.MainData.alpha = reader.GetByte();
                                    reader.Read();
                                }
                            }
                            break;
                        case "bounceForwardLimit":
                            float bounceForward = reader.GetSingle();
                            keyframe.bounceForwardLimit = bounceForward;
                            break;
                        case "bounceBackLimit":
                            float bounceBack = reader.GetSingle();
                            keyframe.bounceBackLimit = bounceBack;
                            break;
                        case "frameTime":
                            float frameTime = reader.GetSingle();
                            keyframe.frameTime = frameTime;
                            break;
                        case "easingMethod":
                            short easingMethod = reader.GetInt16();
                            keyframe.easingMethod = easingMethod;
                            break;
                        case "interpolationType":
                            short interpolationType = reader.GetInt16();
                            keyframe.interpolationType = interpolationType;
                            break;
                    }
                }
            }
            throw new JsonException();
        }

        public override void Write(
            Utf8JsonWriter writer, Keyframe keyframe, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            if (keyframe is IntKey intKey)
            {
                writer.WriteNumber("keyframeType", 1);
                writer.WriteStartObject("MainData");
                writer.WriteNumber("value", intKey.MainData.value);
                writer.WriteEndObject();

            }
            else if (keyframe is FloatKey floatKey)
            {
                writer.WriteNumber("keyframeType", 2);
                writer.WriteStartObject("MainData");
                writer.WriteNumber("value", floatKey.MainData.value);
                writer.WriteEndObject();
            }
            else if (keyframe is ColorKey colorKey)
            {
                writer.WriteNumber("keyframeType", 3);
                writer.WriteStartObject("MainData");
                writer.WriteNumber("red", colorKey.MainData.red);
                writer.WriteNumber("green", colorKey.MainData.red);
                writer.WriteNumber("blue", colorKey.MainData.red);
                writer.WriteNumber("alpha", colorKey.MainData.red);
                writer.WriteEndObject();
            }
            writer.WriteNumber("bounceForwardLimit", keyframe.bounceForwardLimit);
            writer.WriteNumber("bounceBackLimit", keyframe.bounceBackLimit);
            writer.WriteNumber("frameTime", keyframe.frameTime);
            writer.WriteNumber("easingMethod", keyframe.easingMethod);
            writer.WriteNumber("interpolationType", keyframe.interpolationType);
            writer.WriteEndObject();
        }

        /*public override void Write(
    Utf8JsonWriter writer,
    Keyframe value,
    JsonSerializerOptions options)
        {
            switch (value)
            {
                case null:
                    JsonSerializer.Serialize(writer, (Keyframe)null, options);
                    break;
                default:
                    {
                        var type = value.GetType();
                        Console.WriteLine(type);
                        JsonSerializer.Serialize(writer, value, type, options);
                        break;
                    }
            }
        }
    }*/
        [AttributeUsage(AttributeTargets.Interface, AllowMultiple = false)]
        public class JsonInterfaceConverterAttribute : JsonConverterAttribute
        {
            public JsonInterfaceConverterAttribute(Type converterType)
                : base(converterType)
            {
            }
        }
    }
}
