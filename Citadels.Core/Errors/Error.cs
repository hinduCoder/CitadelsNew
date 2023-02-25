namespace Citadels.Core.Errors;

public class Error
{
    public int Code { get; }
    public string Message { get; }

    public Error(string message)
    {
        Code = DefaultErrorCode;
        Message = message;
    }

    public Error(int code, string message)
    {
        Code = code;
        Message = message;
    }

    public static readonly int DefaultErrorCode = 0;
}