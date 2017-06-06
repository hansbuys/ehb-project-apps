using Ehb.Dijlezonen.Kassa.App.Shared.Services;

namespace Ehb.Dijlezonen.Kassa.App.Droid
{
    public class AndroidBackendConfiguration : IBackendConfiguration
    {
        public string BaseUrl => "http://10.0.2.2:44307/api";
    }
}