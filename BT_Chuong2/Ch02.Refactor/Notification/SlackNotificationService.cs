namespace Ch02.Refactor.Notification;

public class SlackNotificationService : INotificationService
{
    public void Send(string to, string message) =>
        Console.WriteLine($"  [SLACK] to {to}: {message}");
}
