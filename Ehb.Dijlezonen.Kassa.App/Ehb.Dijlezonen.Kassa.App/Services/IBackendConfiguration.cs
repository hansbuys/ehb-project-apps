namespace Ehb.Dijlezonen.Kassa.App.Shared.Services
{
    public interface IBackendConfiguration
    {
        /// <summary>
        /// Provides the base URL for connectivity with the backend rest 
        /// </summary>
        string BaseUrl { get; }
    }

    public class BackendConfiguration : IBackendConfiguration
    {
        public string BaseUrl => "http://localhost:44307/api";
    }
}