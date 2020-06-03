using System;

namespace Hostly
{
    /// <summary>
    /// Represents an entry point for a specific device platform i.e. `FormsAppCompatActivity` in Android or `FormsApplicationDelegate` in IOS
    /// </summary>
    public interface IXamarinHostingPlatform
    {
        /// <summary>
        /// Event fired when platform has been created
        /// </summary>
        event EventHandler OnCreated;
        /// <summary>
        /// Event fired when platform has been destroyed (this is only avaliable)
        /// </summary>
        event EventHandler OnDestroyed;
        /// <summary>
        /// Event fired when platform has started
        /// </summary>
        event EventHandler OnStarted;
        /// <summary>
        /// Event fired when platform has been suspended
        /// </summary>
        event EventHandler OnStopped;
        /// <summary>
        /// Event fired when platform has become deactivated before being moved into the background (this is only avaliable in iOS)
        /// </summary>
        event EventHandler OnDeactivate;
        /// <summary>
        /// Event fired when platform has entered the background
        /// </summary>
        event EventHandler OnPause;
        /// <summary>
        /// Event fired when platform has entered the foreground before becoming active (this is only avaliable in iOS)
        /// </summary>
        event EventHandler OnEnterForeground;
        /// <summary>
        /// Event fired when platform has become active in the foreground
        /// </summary>
        event EventHandler OnResume;
        /// <summary>
        /// Entry point to load <see cref="IXamarinApplication"/>
        /// </summary>
        void LoadApplication(object application);
    }
}
