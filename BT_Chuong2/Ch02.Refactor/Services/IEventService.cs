using Ch02.Refactor.Domain;
using Ch02.Refactor.Patterns.Observers;

namespace Ch02.Refactor.Services;

public interface IEventService
{
    void Subscribe(IEventObserver observer);
    void Unsubscribe(IEventObserver observer);
    Event CreateEvent(Guid clubId, string title, DateTime date);
    void Register(Guid eventId, Guid memberId);
    void CheckIn(Guid eventId, Guid memberId);
}
