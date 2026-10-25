namespace Ch02.Refactor.Logging;

public class ConsoleLogger : IAppLogger
{
	public void Info(string message) => Console.WriteLine($"[LOG] {message}");
	
}
