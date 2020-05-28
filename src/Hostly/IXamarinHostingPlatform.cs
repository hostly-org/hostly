using System;
using Xamarin.Forms;

namespace Hostly
{
    public interface IXamarinHostingPlatform
    {
        event EventHandler OnStarted;
        event EventHandler OnStopped;
        void LoadApplication(IXamarinApplication application);
    }
}
