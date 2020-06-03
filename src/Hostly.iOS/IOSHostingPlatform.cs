
using System;
using System.Linq;
using System.Reflection;
using UIKit;
using Xamarin.Forms.Platform.iOS;

namespace Hostly.IOS
{
    internal sealed class IOSHostingPlatform : IXamarinHostingPlatform
    {
        private readonly FormsApplicationDelegate _activity;
        public event EventHandler OnStarted;
        public event EventHandler OnStopped;
        public event EventHandler OnDeactivate;
        public event EventHandler OnPause;
        public event EventHandler OnEnterForeground;
        public event EventHandler OnResume;
        public event EventHandler OnCreated;
        public event EventHandler OnDestroyed;

        public IOSHostingPlatform(FormsApplicationDelegate activity)
        {
            _activity = activity;

            UIApplication.Notifications.ObserveDidFinishLaunching((sender, args) => OnCreated(this, null));
            UIApplication.Notifications.ObserveDidBecomeActive((sender, args) => OnStarted(this, null));
            UIApplication.Notifications.ObserveWillResignActive((sender, args) => OnDeactivate(this, null));
            UIApplication.Notifications.ObserveDidEnterBackground((sender, args) => OnPause(this, null));
            UIApplication.Notifications.ObserveWillEnterForeground((sender, args) => OnEnterForeground(this, null));
            UIApplication.Notifications.ObserveDidBecomeActive((sender, args) => OnResume(this, null));
            UIApplication.Notifications.ObserveWillTerminate((sender, args) => OnStopped(this, null));
        }

        public void LoadApplication(object application)
        {
            if (typeof(global::Xamarin.Forms.Application).IsAssignableFrom(application.GetType()))
            {
                var methods = typeof(FormsApplicationDelegate).GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic);
                var loadApplicationMethod = methods.Where(method => method.Name.Equals(nameof(IXamarinHostingPlatform.LoadApplication))).FirstOrDefault();

                loadApplicationMethod.Invoke(_activity, new object[] { (global::Xamarin.Forms.Application)application });
            }
            else
            {
                throw new ArgumentException("Application supplied is of incorrect type");
            } 
        }
    }
}
