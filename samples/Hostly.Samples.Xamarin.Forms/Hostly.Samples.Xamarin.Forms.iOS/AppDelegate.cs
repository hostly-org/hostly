using Foundation;
using Hostly.Extensions;
using System;
using UIKit;
using Xamarin.Forms;

namespace Hostly.Samples.Xamarin.Forms.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate, IXamarinHostingPlatform
    {
        public event EventHandler OnStarted;
        public event EventHandler OnStopped;

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
                .UseAppSettings(typeof(Startup).Assembly)
                .UsePlatform(this)
                .Build()
                .StartAsync().Wait();

            return base.FinishedLaunching(app, options);
        }

        public override void FinishedLaunching(UIApplication uiApplication)
        {
            base.FinishedLaunching(uiApplication);
            OnStarted(this, null);
        }

        public override void WillTerminate(UIApplication uiApplication)
        {
            base.WillTerminate(uiApplication);
            OnStopped(this, null);
        }

        void IXamarinHostingPlatform.LoadApplication(IXamarinApplication application)
        {
            if (application is global::Xamarin.Forms.Application)
                base.LoadApplication((global::Xamarin.Forms.Application)application);

            throw new ArgumentException("Application supplied is of incorrect type");
        }
    }
}
