namespace TechincalInterview.OmniaRetail.Contracts.Adapters
{
    public interface ILoggerAdapter<TType>
    {
        void LogDebug(string? message, params object?[] args);

        void LogInformation(string? message, params object?[] args);

        void LogError(Exception exception, string message, params object?[] args);
    }
}
