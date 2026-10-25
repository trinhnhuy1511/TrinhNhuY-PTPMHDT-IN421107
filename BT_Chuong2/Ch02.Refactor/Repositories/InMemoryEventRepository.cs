using Ch02.Refactor.Domain;

namespace Ch02.Refactor.Repositories;

public class InMemoryEventRepository : IEventRepository
{
    private readonly List<Event> _store = new();

    public IReadOnlyList<Event> GetByClub(Guid clubId) =>
        _store.Where(e => e.ClubId == clubId).ToList();
    public Event? GetById(Guid id) => _store.FirstOrDefault(e => e.Id == id);
    public void Add(Event ev) => _store.Add(ev);
}

