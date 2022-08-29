namespace Services.CustomExceptions;

public class ChatAlreadyExistsException : Exception
{
	public ChatAlreadyExistsException()
	{
	}
    public ChatAlreadyExistsException(string message) : base(message)
    {
    }
}
