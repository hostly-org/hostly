using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Hostly.Internals
{
    internal sealed class XamarinNavigation : INavigation
    {
        public IReadOnlyList<Page> ModalStack => XamarinProxies.NavigationProxy?.ModalStack;
        public IReadOnlyList<Page> NavigationStack => XamarinProxies.NavigationProxy?.NavigationStack;
        public void InsertPageBefore(Page page, Page before) => XamarinProxies.NavigationProxy?.InsertPageBefore(page, before);
        public Task<Page> PopAsync() => XamarinProxies.NavigationProxy?.PopAsync();
        public Task<Page> PopAsync(bool animated) => XamarinProxies.NavigationProxy?.PopAsync(animated);
        public Task<Page> PopModalAsync() => XamarinProxies.NavigationProxy?.PopModalAsync();
        public Task<Page> PopModalAsync(bool animated) => XamarinProxies.NavigationProxy?.PopModalAsync(animated);
        public Task PopToRootAsync() => XamarinProxies.NavigationProxy?.PopToRootAsync();
        public Task PopToRootAsync(bool animated) => XamarinProxies.NavigationProxy?.PopToRootAsync(animated);
        public Task PushAsync(Page page) => XamarinProxies.NavigationProxy?.PushAsync(page);
        public Task PushAsync(Page page, bool animated) => XamarinProxies.NavigationProxy?.PushAsync(page, animated);
        public Task PushModalAsync(Page page) => XamarinProxies.NavigationProxy?.PushModalAsync(page);
        public Task PushModalAsync(Page page, bool animated) => XamarinProxies.NavigationProxy?.PushModalAsync(page, animated);
        public void RemovePage(Page page) => XamarinProxies.NavigationProxy?.RemovePage(page);
    }
}
