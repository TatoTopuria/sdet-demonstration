namespace AutomationExercise.Tests.Utilities.Extensions;

public static class StringExtensions
{
    public static bool ContainsIgnoreCase(this string source, string value)
    {
        return source.Contains(value, StringComparison.OrdinalIgnoreCase);
    }

    public static string Truncate(this string source, int maxLength)
    {
        if (source.Length <= maxLength)
            return source;

        return string.Concat(source.AsSpan(0, maxLength), "\u2026");
    }

    public static bool IsNullOrWhiteSpace(this string? source)
    {
        return string.IsNullOrWhiteSpace(source);
    }
}
