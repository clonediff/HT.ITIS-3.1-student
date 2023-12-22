namespace Dotnet.Homeworks.Shared.Dto;

public class Result
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public string? Error { get; }

    public Result(bool isSuccessful, string? error = default)
    {
        IsSuccess = isSuccessful;
        if (error is not null) 
            Error = error;
    }

    public static implicit operator Result(string error)
    {
        return new Result(false, error);
    }
    
    public static implicit operator Result(bool isSuccess)
    {
        return new Result(isSuccess);
    }
}

public class Result<TValue> : Result
{
    private readonly TValue? _value;

    public Result(TValue? val, bool isSuccessful, string? error = default)
        : base(isSuccessful, error)
    {
        _value = val;
    }

    public TValue? Value => IsSuccess
        ? _value
        : throw new Exception(Error);
    
    public static implicit operator Result<TValue>(TValue success)
    {
        return new Result<TValue>(success, true);
    }
    
    public static implicit operator Result<TValue>(string error)
    {
        return new Result<TValue>(default, false, error);
    }
    
    public static implicit operator Result<TValue>(bool isSuccess)
    {
        return new Result<TValue>(default, isSuccess);
    }
}