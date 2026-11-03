using MinimalWebApp.Domain.Common;

namespace MinimalWebApp.Web.Extensions;

/// <summary>
/// The single place where domain failures become HTTP responses.
/// Inner layers stay free of any ASP.NET types.
/// </summary>
public static class ResultExtensions
{
    public static IResult ToProblem(this Error error) => error.Type switch
    {
        ErrorType.Validation => Results.ValidationProblem(
            error.Failures ?? new Dictionary<string, string[]>()),
        ErrorType.NotFound => Results.Problem(
            detail: error.Message, statusCode: StatusCodes.Status404NotFound, title: error.Code),
        ErrorType.Conflict => Results.Problem(
            detail: error.Message, statusCode: StatusCodes.Status409Conflict, title: error.Code),
        _ => Results.Problem(statusCode: StatusCodes.Status500InternalServerError)
    };

    public static IResult Match<TValue>(this Result<TValue> result, Func<TValue, IResult> onSuccess) =>
        result.IsSuccess ? onSuccess(result.Value) : result.Error!.ToProblem();

    public static IResult Match(this Result result, Func<IResult> onSuccess) =>
        result.IsSuccess ? onSuccess() : result.Error!.ToProblem();
}
