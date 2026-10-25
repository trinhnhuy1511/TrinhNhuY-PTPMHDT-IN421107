using Ch02.Refactor.Domain;

namespace Ch02.Refactor.Repositories;

public class InMemoryClubRepository : IClubRepository
{
    private readonly List<Club> _store = new();

    public IReadOnlyList<Club> GetAll() => _store.AsReadOnly();
    public Club? GetById(Guid id) => _store.FirstOrDefault(c => c.Id == id);
    public void Add(Club club) => _store.Add(club);
}
