namespace MinimalWebApp.Domain.Common;

/// <summary>
/// Describes what kind of failure occurred, independent of any transport.
/// Outer layers decide how to surface it (HTTP status, log level, ...).
/// </summary>
public enum ErrorType
{
    Validation,
    NotFound,
    Conflict
}

/// <summary>
/// A failure produced by the domain or application layer.
/// <paramref name="Failures"/> is only populated for <see cref="ErrorType.Validation"/>.
/// </summary>
public sealed record Error(
    ErrorType Type,
    string Code,
    string Message,
    IReadOnlyDictionary<string, string[]>? Failures = null)
{
    public static Error Validation(IReadOnlyDictionary<string, string[]> failures) =>
        new(ErrorType.Validation, "validation_failed", "One or more validation errors occurred.", failures);

    public static Error NotFound(string code, string message) =>
        new(ErrorType.NotFound, code, message);

    public static Error Conflict(string code, string message) =>
        new(ErrorType.Conflict, code, message);
}
