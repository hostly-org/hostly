using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace Hostly
{
    internal sealed class XamarinHostLifetime : IHostLifetime
    {
        private readonly IHostApplicationLifetime _applicationLifetime;

        public XamarinHostLifetime(IHostApplicationLifetime applicationLifetime,
            IXamarinHostingPlatform platform)
        {
            if (applicationLifetime == null)
                throw new ArgumentNullException(nameof(applicationLifetime));
            if (platform == null)
                throw new ArgumentNullException(nameof(platform));

            _applicationLifetime = applicationLifetime;

            platform.OnStarted += (sender, args) => { };
            platform.OnStopped += (sender, args) => StopAsync(new CancellationToken());
        }

        public Task WaitForStartAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _applicationLifetime.StopApplication();
            return Task.CompletedTask;
        }
    }
}
