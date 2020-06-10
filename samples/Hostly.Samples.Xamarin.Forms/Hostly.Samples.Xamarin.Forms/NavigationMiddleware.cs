using System;
using System.Threading.Tasks;
using Hostly.Navigation;

namespace Hostly.Samples.Xamarin.Forms
{
    public class NavigationMiddleware
    {
        public Task OnPushAsync(NavigationContext ctx, PushDelegate push)
        {
            return push(ctx);
        }

        public Task OnPopAsync(NavigationContext ctx, PopDelegate pop)
        {
            return pop(ctx);
        }
    }
}
