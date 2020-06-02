using System.Collections.Generic;
using System.Threading.Tasks;
using Hostly.Navigation;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace Hostly.Internals
{
    public sealed class XamarinNavigationProxy : NavigationProxy
    {
        private readonly INavigation _root;

        internal InsertPageBeforeDelegate InsertPageBeforeDelegate { private get; set; }
        internal PushDelegate PushDelegate { private get; set; }
        internal PopDelegate PopDelegate { private get; set; }
        internal PushModalDelegate PushModalDelegate { private get; set; }
        internal PopModalDelegate PopModalDelegate { private get; set; }
        internal PopToRootDelegate PopToRootDelegate { private get; set; }
        internal RemovePageDelegate RemovePageDelegate { private get; set; }

        public XamarinNavigationProxy(object owner)
        {
            if(typeof(INavigation).IsAssignableFrom(owner.GetType()))
                _root = (INavigation)owner;
            else if (typeof(Application).IsAssignableFrom(owner.GetType()) 
                && typeof(NavigationPage).IsAssignableFrom(((Application)owner).MainPage?.GetType()))
                _root = ((Application)owner).MainPage.Navigation;
        }

        public IReadOnlyList<Page> ModalStack => throw new System.NotImplementedException();

        public IReadOnlyList<Page> NavigationStack => throw new System.NotImplementedException();

        public void InsertPageBefore(Page page, Page before)
        {
            InsertPageBeforeDelegate(new NavigationContext(page, before));
            _root.InsertPageBefore(page, before);
        }

        public async Task<Page> PopAsync()
        {
            await PopDelegate(new NavigationContext());
            return await _root.PopAsync();
        }

        public async Task<Page> PopAsync(bool animated)
        {
            await PopDelegate(new NavigationContext(animated));
            return await _root.PopAsync();
        }

        public async Task<Page> PopModalAsync()
        {
            await PopModalDelegate(new NavigationContext());
            return await _root.PopModalAsync();
        }

        public async Task<Page> PopModalAsync(bool animated)
        {
            await PopModalDelegate(new NavigationContext(animated));
            return await _root.PopModalAsync(animated);
        }

        public async Task PopToRootAsync()
        {
            await PopToRootDelegate(new NavigationContext());
            await _root.PopToRootAsync();
        }

        public async Task PopToRootAsync(bool animated)
        {
            await PopToRootDelegate(new NavigationContext(animated));
            await _root.PopToRootAsync(animated);
        }

        public async Task PushAsync(Page page)
        {
            await PushDelegate?.Invoke(new NavigationContext(page));
            await _root.PushAsync(page);
        }

        public async Task PushAsync(Page page, bool animated)
        {
            await PushDelegate(new NavigationContext(page, animated));
            await _root.PushAsync(page, animated);
        }

        public async Task PushModalAsync(Page page)
        {
            await PushModalDelegate(new NavigationContext(page));
            await _root.PushModalAsync(page);
        }

        public async Task PushModalAsync(Page page, bool animated)
        {
            await PushModalDelegate(new NavigationContext(page, animated));
            await _root.PushModalAsync(page, animated);
        }

        public void RemovePage(Page page)
        {
            RemovePageDelegate(new NavigationContext(page));
            _root.RemovePage(page);
        }
    }
}
