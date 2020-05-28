using System;
using Xamarin.Forms;

namespace Hostly.Tests.Mocks
{
    public class MockPlatform : IXamarinHostingPlatform
    {
        public event EventHandler OnStarted;
        public event EventHandler OnStopped;

        public bool Loaded { get; private set; }

        public void LoadApplication(IXamarinApplication application) 
        {
            Loaded = true;
        }
    }
}
