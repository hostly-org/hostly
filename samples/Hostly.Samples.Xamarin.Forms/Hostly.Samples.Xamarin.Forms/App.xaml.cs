using Xamarin.Forms;

namespace Hostly.Samples.Xamarin.Forms
{
    public partial class App : Application, IXamarinApplication
    {
        public App()
        {
            InitializeComponent();

            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
