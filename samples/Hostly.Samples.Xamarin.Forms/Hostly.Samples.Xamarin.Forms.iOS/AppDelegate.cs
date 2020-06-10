using Foundation;
using Hostly.Extensions;
using Hostly.IOS.Extensions;
using Microsoft.Extensions.Configuration;
using UIKit;
using Xamarin.Essentials;

namespace Hostly.Samples.Xamarin.Forms.IOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();

            new XamarinHostBuilder()
                .UseApplication<App>()
                .UseStartup<Startup>()
                .UseAppSettings<Startup>()
                .UsePlatform(this)
                .ConfigureHostConfiguration(c => c.AddCommandLine(new string[] { $"ContentRoot={FileSystem.AppDataDirectory}" }))
                .UseNavigationMiddleware<NavigationMiddleware>()
                .Build()
                .StartAsync().Wait();

            return base.FinishedLaunching(app, options);
        }
    }
}
