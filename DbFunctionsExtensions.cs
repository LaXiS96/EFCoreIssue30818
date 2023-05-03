using System.Reflection;

namespace EFCoreIssue30818
{
    internal static class DbFunctionsExtensions
    {
        public static readonly MethodInfo JsonValueMethod = typeof(DbFunctionsExtensions).GetMethod(nameof(JsonValue), BindingFlags.Public | BindingFlags.Static)!;

        public static string? JsonValue(object propertyReference, string path)
            => throw new InvalidOperationException("This function call should have been translated to SQL!");
    }
}
