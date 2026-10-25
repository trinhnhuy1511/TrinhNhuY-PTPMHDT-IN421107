using Ch02.Refactor.Patterns.Strategies;

namespace Ch02.Refactor.Services;

public interface IReportService
{
    string Generate(IReportStrategy strategy);
}
