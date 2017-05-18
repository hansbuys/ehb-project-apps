using Ehb.Dijlezonen.Kassa.App.Shared.Model;
using Ehb.Dijlezonen.Kassa.App.Testing;
using Xunit.Abstractions;

namespace Ehb.Dijlezonen.Kassa.App.Tests.BarcodeScanner
{
    public class BarcodeScannerViewModelTests : ViewModelTest<BarcodeScannerViewModel>
    {
        public BarcodeScannerViewModelTests(ITestOutputHelper output) : base(output)
        {
        }
    }
}
