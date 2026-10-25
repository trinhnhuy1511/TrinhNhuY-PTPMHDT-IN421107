using Ch02.Refactor.Domain;

namespace Ch02.Refactor.Repositories;

public interface IMemberRepository
{
    IReadOnlyList<Member> GetByClub(Guid clubId);
    Member? GetById(Guid id);
    void Add(Member member);
}
