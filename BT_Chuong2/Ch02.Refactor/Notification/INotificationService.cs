namespace Ch02.Refactor.Notification;

public interface INotificationService
{
	void Send(string to, string message);
}
