using Ch02.Refactor.Domain;

namespace Ch02.Refactor.Patterns.Observers;

public interface IEventObserver
{
    void OnEventCreated(Event ev, Club club);
}
