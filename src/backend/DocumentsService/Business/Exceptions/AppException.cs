namespace Business.Exceptions;

public class AppException : Exception
{
    public int StatusCode { get; set; }

    protected AppException(string message, int statusCode) : base(message)
    {
        StatusCode = statusCode;
    }
}
