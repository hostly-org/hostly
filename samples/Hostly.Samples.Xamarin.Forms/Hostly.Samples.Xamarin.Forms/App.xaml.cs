using System;
using Xamarin.Forms;

namespace Hostly.Samples.Xamarin.Forms
{
    public partial class App : Application
    {
        public App(MainPage mainPage)
        {
            InitializeComponent();
            MainPage = new NavigationPage(mainPage);
        }
    }
}
