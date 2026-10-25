using Ch02.Refactor.Domain;
using Ch02.Refactor.Logging;

namespace Ch02.Refactor.Patterns.Observers;

public class LogNotifier : IEventObserver
{
    private readonly IAppLogger _logger;
    public LogNotifier(IAppLogger logger) => _logger = logger;

    public void OnEventCreated(Event ev, Club club) =>
        _logger.Info($"Event created: '{ev.Title}' ({club.Name})");
}
