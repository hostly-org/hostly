using System;

namespace Hostly
{
    /// <summary>
    /// Represents an entry point for a specific device platform i.e. `FormsAppCompatActivity` in Android or `FormsApplicationDelegate` in IOS
    /// </summary>
    public interface IXamarinHostingPlatform
    {
        /// <summary>
        /// Event fired when platform has started
        /// </summary>
        event EventHandler OnStarted;
        /// <summary>
        /// Event fired when platform has stopped
        /// </summary
        event EventHandler OnStopped;
        /// <summary>
        /// Entry point to load <see cref="IXamarinApplication"/>
        /// </summary
        void LoadApplication(IXamarinApplication application);
    }
}
