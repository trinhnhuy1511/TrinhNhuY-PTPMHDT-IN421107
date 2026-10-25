using Ch02.Refactor.Domain;

namespace Ch02.Refactor.Repositories;

public class InMemoryMemberRepository : IMemberRepository
{
    private readonly List<Member> _store = new();

    public IReadOnlyList<Member> GetByClub(Guid clubId) =>
        _store.Where(m => m.ClubId == clubId).ToList();
    public Member? GetById(Guid id) => _store.FirstOrDefault(m => m.Id == id);
    public void Add(Member member) => _store.Add(member);
}
