namespace Ehb.Dijlezonen.Kassa.App.Shared.Services
{
    public interface IBackendConfiguration
    {
        /// <summary>
        /// Provides the base URL for connectivity with the backend rest 
        /// </summary>
        string BaseUrl { get; }
    }
}