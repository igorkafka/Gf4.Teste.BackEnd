﻿
namespace PedidoStore.Core.Extensions
{
    public static class GenericTypeExtensions
    {
        public static bool IsDefault<T>(this T value) =>
            Equals(value, default(T));

        public static string GetGenericTypeName(this object @object)
        {
            var type = @object.GetType();

            // Check if the type is not generic
            if (!type.IsGenericType)
                return type.Name;

            // Get the names of the generic arguments and join them with commas
            var genericTypes = string.Join(",", type.GetGenericArguments().Select(t => t.Name).ToArray());

            // Remove the backtick and append the generic arguments to the type name
            return $"{type.Name[..type.Name.IndexOf('`')]}<{genericTypes}>";
        }
    }
}
