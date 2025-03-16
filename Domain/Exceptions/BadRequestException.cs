namespace Domain.Exceptions;

public class BadRequestException : Exception
{
    public BadRequestException(string message)
        : base(message)
    {
    }

    public BadRequestException(string message, object arg0)
        : base(string.Format(message, arg0))
    {
    }

    public BadRequestException(string message, object arg0, object arg1)
        : base(string.Format(message, arg0, arg1))
    {
    }

    public BadRequestException(string message, object arg0, object arg1, object arg2)
        : base(string.Format(message, arg0, arg1, arg2))
    {
    }

    public BadRequestException(string message, params object[] args)
        : base(string.Format(message, args))
    {
    }
}