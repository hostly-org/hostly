using System;
using Windows.UI.Xaml;

namespace Hostly.UWP
{
    internal sealed class UWPHostingPlatform : IXamarinHostingPlatform
    {
        private readonly Application _app;

        public event EventHandler<object> OnStarted;
        public event EventHandler<object> OnStopped;
        public event EventHandler<object> OnDeactivate;
        public event EventHandler<object> OnPause;
        public event EventHandler<object> OnEnterForeground;
        public event EventHandler<object> OnResume;
        public event EventHandler<object> OnCreated;
        public event EventHandler<object> OnDestroyed;

        public UWPHostingPlatform(Application app)
        {
            _app = app;
            _app.Resuming += (sender, args) => OnResume(sender, args);
            _app.Suspending += (sender, args) => OnStopped(sender, args);
            _app.LeavingBackground += (sender, args) => OnEnterForeground(sender, args);
            _app.EnteredBackground += (sender, args) => OnPause(sender, args);
        }

        public void LoadApplication(object application)
        {
            return;
        }
    }
}
