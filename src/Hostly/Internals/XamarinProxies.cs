namespace Hostly.Internals
{
    public static class XamarinProxies
    {
        public static XamarinNavigationProxy NavigationProxy { get; set; }

        public static void SetNavigationProxy(XamarinNavigationProxy proxy)
        {
            // This is only hit in the auto proxy setup, if UseNavigationRoot has been called then this will not be executed.
            if(NavigationProxy == null)
                NavigationProxy = proxy;
        }
    }
}
