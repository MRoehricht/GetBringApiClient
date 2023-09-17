namespace GetBringApiClient.Exceptions;

public class LoginException : Exception
{
    public LoginException() : base(Constants.ErrorFirstLogin) { }

    public LoginException(string? message) : base(message) { }
}
