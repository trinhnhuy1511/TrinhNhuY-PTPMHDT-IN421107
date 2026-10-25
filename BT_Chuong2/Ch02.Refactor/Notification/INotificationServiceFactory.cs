namespace Ch02.Refactor.Notification;

public interface INotificationServiceFactory
{
	INotificationService Create(NotificationChannel channel);
}
