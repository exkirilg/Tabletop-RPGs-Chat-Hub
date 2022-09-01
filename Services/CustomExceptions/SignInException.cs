namespace Services.CustomExceptions;

public class SignInException : Exception
{
    public SignInException()
    {
    }
    public SignInException(string message) : base(message)
    {
    }
}
