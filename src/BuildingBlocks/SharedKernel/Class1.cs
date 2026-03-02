namespace BuildingBlocks.SharedKernel;

public sealed record Error(string Code, string Message)
{
    public static Error None { get; } = new("None", string.Empty);
}

public class Result
{
    protected Result(bool isSuccess, Error error)
    {
        IsSuccess = isSuccess;
        Error = error;
    }

    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public Error Error { get; }

    public static Result Success() => new(true, Error.None);
    public static Result Failure(Error error) => new(false, error);
}

public sealed class Result<T> : Result
{
    private readonly T? _value;

    private Result(bool isSuccess, Error error, T? value)
        : base(isSuccess, error)
    {
        _value = value;
    }

    public T Value => IsSuccess
        ? _value!
        : throw new InvalidOperationException("Cannot access Value of a failed result.");

    public static Result<T> Success(T value) => new(true, Error.None, value);
    public static new Result<T> Failure(Error error) => new(false, error, default);
}

public static class Guard
{
    public static string AgainstNullOrWhiteSpace(string? value, string paramName)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Value cannot be null or whitespace.", paramName);
        }

        return value;
    }
}
