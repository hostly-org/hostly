using System;
using Xamarin.Forms;

namespace Hostly.Samples.Xamarin.Forms
{
    public partial class App : Application
    {
        public App(MainPage mainPage)
        {
            InitializeComponent();

            MainPage = mainPage;
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
