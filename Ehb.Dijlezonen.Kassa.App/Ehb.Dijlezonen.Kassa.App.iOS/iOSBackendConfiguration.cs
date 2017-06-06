using Ehb.Dijlezonen.Kassa.App.Shared.Services;

namespace Ehb.Dijlezonen.Kassa.App.iOS
{
    public class iOSBackendConfiguration : IBackendConfiguration
    {
        public string BaseUrl => "http://localhost:44307/api";
    }
}