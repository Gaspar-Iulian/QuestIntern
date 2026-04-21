namespace Ecommerce.Api.Responses;

public sealed class ValidationErrorResponse
{
    public string Message { get; init; } = string.Empty;
    public IReadOnlyList<string> Errors { get; init; } = [];
}
