
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
        public event EventHandler<object> OnStarted;
        public event EventHandler<object> OnStopped;
        public event EventHandler<object> OnDeactivate;
        public event EventHandler<object> OnPause;
        public event EventHandler<object> OnEnterForeground;
        public event EventHandler<object> OnResume;
        public event EventHandler<object> OnCreated;
        public event EventHandler<object> OnDestroyed;

        public IOSHostingPlatform(FormsApplicationDelegate activity)
        {
            _activity = activity;

            UIApplication.Notifications.ObserveDidFinishLaunching((sender, args) => OnCreated(this, args));
            UIApplication.Notifications.ObserveDidBecomeActive((sender, args) => OnStarted(this, args));
            UIApplication.Notifications.ObserveWillResignActive((sender, args) => OnDeactivate(this, args));
            UIApplication.Notifications.ObserveDidEnterBackground((sender, args) => OnPause(this, args));
            UIApplication.Notifications.ObserveWillEnterForeground((sender, args) => OnEnterForeground(this, args));
            UIApplication.Notifications.ObserveDidBecomeActive((sender, args) => OnResume(this, args));
            UIApplication.Notifications.ObserveWillTerminate((sender, args) => OnStopped(this, args));
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
