
using System;
using System.Linq;
using System.Reflection;
using Android.App;
using Android.Arch.Lifecycle;
using Java.Interop;
using Xamarin.Forms.Platform.Android;

namespace Hostly.Android
{
    internal sealed class AndroidHostingPlatform : Application, ILifecycleObserver, IXamarinHostingPlatform
    {
        private readonly FormsAppCompatActivity _app;

        public event EventHandler OnStarted;
        public event EventHandler OnStopped;
        public event EventHandler OnDeactivate;
        public event EventHandler OnPause;
        public event EventHandler OnEnterForeground;
        public event EventHandler OnResume;
        public event EventHandler OnCreated;
        public event EventHandler OnDestroyed;

        public AndroidHostingPlatform(FormsAppCompatActivity app)
        {
            _app = app;
            _app.Lifecycle.AddObserver(this);
        }

        [Lifecycle.Event.OnCreate]
        [Export]
        public void Created()
        {
            OnCreated(this, new EventArgs());
        }

        [Lifecycle.Event.OnDestroy]
        [Export]
        public void Destroyed()
        {
            OnDestroyed(this, new EventArgs());
        }

        [Lifecycle.Event.OnPause]
        [Export]
        public void Pasued()
        {
            OnPause(this, new EventArgs());
        }

        [Lifecycle.Event.OnResume]
        [Export]
        public void Resumed()
        {
            OnPause(this, new EventArgs());
        }

        [Lifecycle.Event.OnStop]
        [Export]
        public void Stopped()
        {
            OnStopped(this, new EventArgs());
        }

        [Lifecycle.Event.OnStart]
        [Export]
        public void Started()
        {
            OnStarted(this, new EventArgs());
        }


        public void LoadApplication(object application)
        {
            if (typeof(global::Xamarin.Forms.Application).IsAssignableFrom(application.GetType()))
            {
                var methods = typeof(FormsAppCompatActivity).GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic);
                var loadApplicationMethod = methods.Where(method => method.Name.Equals(nameof(IXamarinHostingPlatform.LoadApplication))).FirstOrDefault();

                loadApplicationMethod.Invoke(_app, new object[] { (global::Xamarin.Forms.Application)application });
            }
            else
            {
                throw new ArgumentException("Application supplied is of incorrect type");
            } 
        }
    }
}
