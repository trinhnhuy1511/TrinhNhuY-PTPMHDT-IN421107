namespace Ch02.Refactor.Notification;

public class SmsNotificationService : INotificationService
{
    public void Send(string to, string message) =>
        Console.WriteLine($"  [SMS] to {to}: {message}");
}
