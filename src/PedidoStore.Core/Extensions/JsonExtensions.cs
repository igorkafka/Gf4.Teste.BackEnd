﻿using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

namespace PedidoStore.Core.Extensions
{
    public static class JsonExtensions
    {
        private static readonly Lazy<JsonSerializerOptions> LazyOptions =
            new(() => new JsonSerializerOptions().Configure(), isThreadSafe: true);

        public static T FromJson<T>(this string value) =>
            value != null ? JsonSerializer.Deserialize<T>(value, LazyOptions.Value) : default;

        public static string ToJson<T>(this T value) =>
            !value.IsDefault() ? JsonSerializer.Serialize(value, LazyOptions.Value) : default;

        public static JsonSerializerOptions Configure(this JsonSerializerOptions jsonSettings)
        {
            jsonSettings.IncludeFields = true;
            jsonSettings.WriteIndented = false;
            jsonSettings.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            jsonSettings.ReadCommentHandling = JsonCommentHandling.Skip;
            jsonSettings.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            jsonSettings.TypeInfoResolver = new PrivateConstructorContractResolver();
            jsonSettings.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
            return jsonSettings;
        }
    }

    internal sealed class PrivateConstructorContractResolver : DefaultJsonTypeInfoResolver
    {
        public override JsonTypeInfo GetTypeInfo(Type type, JsonSerializerOptions options)
        {
            var jsonTypeInfo = base.GetTypeInfo(type, options);

            // Check if the type is an object, has no public constructor, and CreateObject is not already set
            if (jsonTypeInfo.Kind == JsonTypeInfoKind.Object
                && jsonTypeInfo.CreateObject is null
                && jsonTypeInfo.Type.GetConstructors(BindingFlags.Public | BindingFlags.Instance).Length == 0)
            {
                // Set CreateObject to a lambda expression that creates an instance using a private constructor
                jsonTypeInfo.CreateObject = () => Activator.CreateInstance(jsonTypeInfo.Type, true);
            }

            return jsonTypeInfo;
        }

    }
}