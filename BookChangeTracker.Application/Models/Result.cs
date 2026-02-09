namespace BookChangeTracker.Application.Models;

public abstract record Result
{
    private Result() { }
    
    public sealed record SuccessResult<T>(T Data) : Result;
    public sealed record SuccessResult : Result;
    public sealed record FailureResult(Error Error) : Result;
    
    public static Result Success<T>(T data) => new SuccessResult<T>(data);
    public static Result Success() => new SuccessResult();
    public static Result Failure(Error error) => new FailureResult(error);
}
