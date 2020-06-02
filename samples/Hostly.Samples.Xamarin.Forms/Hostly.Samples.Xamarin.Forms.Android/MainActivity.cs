using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Hostly.Android.Extensions;
using Hostly.Extensions;
using Microsoft.Extensions.Configuration;
using Xamarin.Essentials;

namespace Hostly.Samples.Xamarin.Forms.Droid
{


    [Activity(Label = "Hostly.Samples.Xamarin.Forms", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
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
                .UseAppSettings<Startup>()
                .UsePlatform(this)
                .ConfigureHostConfiguration(c => c.AddCommandLine(new string[] { $"ContentRoot={FileSystem.AppDataDirectory}" }))
                .ConfigureNavigationConfiguration((ctx, sp, b) => {
                    b.UseMiddleware<NavigationMiddleware>();
                })
                .Build()
                .StartAsync().Wait();
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}
