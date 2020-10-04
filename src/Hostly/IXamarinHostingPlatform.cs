using System;

namespace Hostly
{
    /// <summary>
    /// Represents an entry point for a specific device platform i.e. `FormsAppCompatActivity` in Android or `FormsApplicationDelegate` in IOS
    /// </summary>
    public interface IXamarinHostingPlatform
    {
        /// <summary>
        /// Event fired when platform has been created (Android, iOS)
        /// </summary>
        event EventHandler<object> OnCreated;
        /// <summary>
        /// Event fired when platform has been destroyed (Android)
        /// </summary>
        event EventHandler<object> OnDestroyed;
        /// <summary>
        /// Event fired when platform has started (Android, iOS)
        /// </summary>
        event EventHandler<object> OnStarted;
        /// <summary>
        /// Event fired when platform has been suspended (Android, iOS, UWP)
        /// </summary>
        event EventHandler<object> OnStopped;
        /// <summary>
        /// Event fired when platform has become deactivated before being moved into the background (iOS, UWP)
        /// </summary>
        event EventHandler<object> OnDeactivate;
        /// <summary>
        /// Event fired when platform has entered the background (Android, iOS, UWP)
        /// </summary>
        event EventHandler<object> OnPause;
        /// <summary>
        /// Event fired when platform has entered the foreground before becoming active (iOS, UWP)
        /// </summary>
        event EventHandler<object> OnEnterForeground;
        /// <summary>
        /// Event fired when platform has become active in the foreground (Android, iOS, UWP)
        /// </summary>
        event EventHandler<object> OnResume;
        /// <summary>
        /// Entry point to load <see cref="IXamarinApplication"/>
        /// </summary>
        void LoadApplication(object application);
    }
}
