namespace RedeSocial.Application.Validations.Core;

public class Notification
{
    public Notification(string message)
    {
        Message = message;
    }

    public string Message { get; }
}