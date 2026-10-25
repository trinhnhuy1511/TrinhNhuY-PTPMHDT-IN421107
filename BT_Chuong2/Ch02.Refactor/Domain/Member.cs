namespace Ch02.Refactor.Domain;

public class Member
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public Guid ClubId { get; set; }
}
