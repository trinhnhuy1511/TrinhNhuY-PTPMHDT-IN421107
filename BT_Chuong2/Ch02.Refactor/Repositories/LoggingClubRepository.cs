using Ch02.Refactor.Domain;
using Ch02.Refactor.Logging;

namespace Ch02.Refactor.Repositories;

public class LoggingClubRepository : IClubRepository
{
    private readonly IClubRepository _inner;
    private readonly IAppLogger _logger;

    public LoggingClubRepository(IClubRepository inner, IAppLogger logger)
    {
        _inner = inner;
        _logger = logger;
    }

    public IReadOnlyList<Club> GetAll()
    {
        _logger.Info("[repo] GetAll clubs");
        return _inner.GetAll();
    }

    public Club? GetById(Guid id)
    {
        _logger.Info($"[repo] GetById {id}");
        return _inner.GetById(id);
    }

    public void Add(Club club)
    {
        _logger.Info($"[repo] Add club '{club.Name}'");
        _inner.Add(club);
    }
}
