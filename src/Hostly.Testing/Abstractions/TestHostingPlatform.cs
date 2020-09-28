using System;
using Hostly;

namespace Hosting.Testing.Abstractions
{
    internal sealed class TestHostingPlatform : IXamarinHostingPlatform
    {
        public event EventHandler OnStarted;
        public event EventHandler OnStopped;
        public event EventHandler OnCreated;
        public event EventHandler OnDestroyed;
        public event EventHandler OnDeactivate;
        public event EventHandler OnPause;
        public event EventHandler OnEnterForeground;
        public event EventHandler OnResume;

        public void LoadApplication(object application)
        {
        }
    }
}
