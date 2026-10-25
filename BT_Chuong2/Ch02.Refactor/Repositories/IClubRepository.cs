using Ch02.Refactor.Domain;

namespace Ch02.Refactor.Repositories;

public interface IClubRepository
{
    IReadOnlyList<Club> GetAll();
    Club? GetById(Guid id);
    void Add(Club club);
}
