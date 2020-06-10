using Xamarin.Forms;

namespace Hostly.Navigation
{
    public class NavigationContext
    {
        public Page Page { get; }
        public Page Before { get; }
        public bool Animated { get; }

        internal NavigationContext(Page page)
        {
            Page = page;
        }

        internal NavigationContext(Page page, Page before)
        {
            Page = page;
            Before = before;
        }

        internal NavigationContext(Page page, bool animated)
        {
            Page = page;
            Animated = animated;
        }

        internal NavigationContext(bool animated)
        {
            Animated = animated;
        }

        internal NavigationContext()
        {

        }
    }
}
