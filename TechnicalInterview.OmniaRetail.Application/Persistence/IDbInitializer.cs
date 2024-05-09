namespace TechnicalInterview.OmniaRetail.Application.Persistence
{
    public interface IDbInitializer
    {
        public Task InitializeAsync();
    }
}