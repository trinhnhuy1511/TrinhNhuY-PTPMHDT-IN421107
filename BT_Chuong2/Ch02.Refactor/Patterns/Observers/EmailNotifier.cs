using Ch02.Refactor.Domain;
using Ch02.Refactor.Notification;
using Ch02.Refactor.Repositories;

namespace Ch02.Refactor.Patterns.Observers;

public class EmailNotifier : IEventObserver
{
    private readonly IMemberRepository _members;
    private readonly INotificationService _notifier;

    public EmailNotifier(IMemberRepository members, INotificationService notifier)
    {
        _members = members;
        _notifier = notifier;
    }

    public void OnEventCreated(Event ev, Club club)
    {
        foreach (var m in _members.GetByClub(club.Id))
            _notifier.Send(m.Email, $"New event '{ev.Title}' at {club.Name} on {ev.Date:dd/MM/yyyy}");
    }
}
