using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace Hostly
{
    internal sealed class XamarinHostLifetime : IHostLifetime
    {
        public Task WaitForStartAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
