namespace Ehb.Dijlezonen.Kassa.App.UWP
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            InitializeComponent();

            LoadApplication(new Shared.App(new Builder()));
        }
    }
}
