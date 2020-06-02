using System.Collections.Generic;
using System.Threading.Tasks;
using Hostly.Navigation;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace Hostly.Internals
{
    public sealed class XamarinNavigationProxy : NavigationProxy
    {
        private readonly NavigationPage _root;

        internal readonly List<PushDelegate> _pushDelegates;
        internal readonly List<PushModalDelegate> _pushModalDelegates;

        public XamarinNavigationProxy(object owner)
        {
            if(typeof(NavigationPage).IsAssignableFrom(owner.GetType()))
                _root = (NavigationPage)owner;
            else if (typeof(Application).IsAssignableFrom(owner.GetType()) 
                && typeof(NavigationPage).IsAssignableFrom(((Application)owner).MainPage?.GetType()))
                _root = (NavigationPage)((Application)owner).MainPage;
        }

        public IReadOnlyList<Page> ModalStack => throw new System.NotImplementedException();

        public IReadOnlyList<Page> NavigationStack => throw new System.NotImplementedException();

        public void InsertPageBefore(Page page, Page before)
        {
            throw new System.NotImplementedException();
        }

        public Task<Page> PopAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task<Page> PopAsync(bool animated)
        {
            throw new System.NotImplementedException();
        }

        public Task<Page> PopModalAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task<Page> PopModalAsync(bool animated)
        {
            throw new System.NotImplementedException();
        }

        public Task PopToRootAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task PopToRootAsync(bool animated)
        {
            throw new System.NotImplementedException();
        }

        public async Task PushAsync(Page page)
        {
            foreach (var push in _pushDelegates)
                await push(new NavigationContext());

            await _root.PushAsync(page);
        }

        public Task PushAsync(Page page, bool animated)
        {
            throw new System.NotImplementedException();
        }

        public async Task PushModalAsync(Page page)
        {
            foreach (var pushModal in _pushModalDelegates)
                await pushModal(new NavigationContext());

            await _root.PushModalAsync(page);
        }

        public Task PushModalAsync(Page page, bool animated)
        {
            throw new System.NotImplementedException();
        }

        public void RemovePage(Page page)
        {
            throw new System.NotImplementedException();
        }
    }
}
