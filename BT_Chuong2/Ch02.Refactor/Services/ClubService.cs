using Ch02.Refactor.Domain;
using Ch02.Refactor.Logging;
using Ch02.Refactor.Notification;
using Ch02.Refactor.Repositories;

namespace Ch02.Refactor.Services;

public class ClubService : IClubService
{
    private readonly IClubRepository _clubs;
    private readonly IMemberRepository _members;
    private readonly INotificationService _notifier;
    private readonly IAppLogger _logger;

    public ClubService(IClubRepository clubs, IMemberRepository members,
        INotificationService notifier, IAppLogger logger)
    {
        _clubs = clubs;
        _members = members;
        _notifier = notifier;
        _logger = logger;
    }

    public Club CreateClub(string name, string description, string presidentEmail)
    { 
        var club = Club.Create(name, description);
        _clubs.Add(club);
        _logger.Info($"Club created: {name}");
        _notifier.Send(presidentEmail, $"Club '{name}' has been created");
        return club;
    }

    public Member AddMember(Guid clubId, string memberName, string email)
    {
        var club = _clubs.GetById(clubId)
            ?? throw new InvalidOperationException("Club not found");

        if (!club.HasRoom(_members.GetByClub(clubId).Count))
            throw new InvalidOperationException($"Club is full (max {Club.MaxMembers} members)");

        var member = new Member { Id = Guid.NewGuid(), Name = memberName, Email = email, ClubId = clubId };
        _members.Add(member);
        _logger.Info($"Member {memberName} joined {club.Name}");
        _notifier.Send(email, $"Welcome to {club.Name}!");
        return member;
    }
}
