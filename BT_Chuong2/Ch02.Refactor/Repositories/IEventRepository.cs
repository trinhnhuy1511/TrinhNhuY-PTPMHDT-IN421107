using Ch02.Refactor.Domain;

namespace Ch02.Refactor.Repositories;

public interface IEventRepository
{
    IReadOnlyList<Event> GetByClub(Guid clubId);
    Event? GetById(Guid id);
    void Add(Event ev);
}
