using Ch02.Refactor.Domain;

namespace Ch02.Refactor.Services;

public interface IClubService
{
    Club CreateClub(string name, string description, string presidentEmail);
    Member AddMember(Guid clubId, string memberName, string email);
}
