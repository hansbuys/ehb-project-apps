using Ehb.Dijlezonen.Kassa.App.Shared.Services;

namespace Ehb.Dijlezonen.Kassa.App.UWP
{
    public class WindowsBackendConfiguration : IBackendConfiguration
    {
        public string BaseUrl => "http://localhost:44307/api";
    }
}