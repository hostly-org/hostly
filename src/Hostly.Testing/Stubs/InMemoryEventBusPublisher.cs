using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using OpenEventSourcing.Events;

namespace Hostly.Testing.Stubs
{
    internal sealed class InMemoryEventBus : IEventBusPublisher, IEventBusConsumer
    {
        private readonly Channel<IEvent> _channel;
        private readonly CancellationTokenSource _stoppingCts = new CancellationTokenSource();
        private readonly object _lock = new object();
        private Task _consumerTask;
        private readonly IServiceProvider _serviceProvider;

        public InMemoryEventBus(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _channel = Channel.CreateUnbounded<IEvent>();
            _consumerTask = Task.Run(() => ExecuteConsumerAsync(_stoppingCts.Token));
        }

        public async Task PublishAsync<TEvent>(TEvent @event) where TEvent : IEvent
        {
            if (_channel.Writer.TryWrite(@event))
                return;

            await _channel.Writer.WriteAsync(@event);
        }
        public async Task PublishAsync(IEnumerable<IEvent> events)
        {
            foreach (var @event in events)
            {
                if (_channel.Writer.TryWrite(@event))
                    continue;

                await _channel.Writer.WriteAsync(@event);
            }
        }

        public Task StartAsync(CancellationToken cancellationToken = default)
        {
            _consumerTask = ExecuteConsumerAsync(_stoppingCts.Token);

            if (_consumerTask.IsCompleted)
                return _consumerTask;

            return Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken = default)
        {
            if (_consumerTask == null)
                return;

            try
            {
                _stoppingCts.Cancel();
            }
            finally
            {
                await Task.WhenAny(_consumerTask, Task.Delay(Timeout.Infinite, cancellationToken));
            }
        }

        private async Task ExecuteConsumerAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                while (await _channel.Reader.WaitToReadAsync(cancellationToken))
                {
                    if (!_channel.Reader.TryRead(out var @event))
                        @event = await _channel.Reader.ReadAsync(cancellationToken);

                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var dispatcher = scope.ServiceProvider.GetRequiredService<IEventDispatcher>();
                        await dispatcher.DispatchAsync(@event).ConfigureAwait(false);
                    }
                }
            }
        }
    }
}
