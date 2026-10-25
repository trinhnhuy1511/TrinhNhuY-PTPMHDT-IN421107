namespace Ch02.Refactor.Patterns.Strategies;

public interface IReportStrategy
{
    string Name { get; }
    string Render(IReadOnlyList<ClubStat> stats);
}
