namespace Services.CustomExceptions;

public class SignUpException : Exception
{
    public SignUpException()
    {
    }
    public SignUpException(string message) : base(message)
    {
    }
}
