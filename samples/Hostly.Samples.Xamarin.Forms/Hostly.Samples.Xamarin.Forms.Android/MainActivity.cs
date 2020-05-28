﻿using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Hostly.Extensions;
using System;
using Xamarin.Essentials;

namespace Hostly.Samples.Xamarin.Forms.Droid
{
    [Activity(Label = "Hostly.Samples.Xamarin.Forms", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity, IXamarinHostingPlatform
    {
        public event EventHandler OnStarted;
        public event EventHandler OnStopped;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            new XamarinHostBuilder()
                .UseApplication<App>()
                .UseStartup<Startup>()
                .UseAppSettings(typeof(Startup).Assembly)
                .UsePlatform(this)
                .Build()
                .StartAsync().Wait();
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
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

        void IXamarinHostingPlatform.LoadApplication(IXamarinApplication application)
        {
            if (application is global::Xamarin.Forms.Application)
                base.LoadApplication((global::Xamarin.Forms.Application)application);

            throw new ArgumentException("Application supplied is of incorrect type");
        }
    }
}