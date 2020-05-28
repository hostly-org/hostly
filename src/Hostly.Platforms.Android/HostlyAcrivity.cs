using Android.OS;
using Hostly.Extensions;
using System;

namespace Hostly.Platforms.Android
{
    public class HostlyActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity, IXamarinHostingPlatform
    {
        public event EventHandler OnStarted;
        public event EventHandler OnStopped;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            new XamarinHostBuilder()
                .UsePlatform(this)
                .Build();
        }

        protected override void OnStop()
        {
            base.OnStop();
            OnStopped(this, null);
        }

        protected override void OnStart()
        {
            base.OnStart();
            OnStarted(this, null);
        }

        void IXamarinHostingPlatform.LoadApplication(Xamarin.Forms.Application application)
        {
            LoadApplication(application);
        }
    }
}