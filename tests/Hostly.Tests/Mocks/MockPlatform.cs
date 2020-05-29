using System;
using Xamarin.Forms;

namespace Hostly.Tests.Mocks
{
    public class MockPlatform : IXamarinHostingPlatform
    {
        public event EventHandler OnStarted;
        public event EventHandler OnStopped;
        public event EventHandler OnCreated;
        public event EventHandler OnDestroyed;
        public event EventHandler OnDeactivate;
        public event EventHandler OnPause;
        public event EventHandler OnEnterForeground;
        public event EventHandler OnResume;

        public bool Loaded { get; private set; }

        public void LoadApplication(IXamarinApplication application) 
        {
            Loaded = true;
        }
    }
}
