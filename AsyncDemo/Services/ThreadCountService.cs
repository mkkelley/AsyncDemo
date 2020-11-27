using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AsyncDemo.Services
{
    public class ThreadCountService : IHostedService, IDisposable
    {
        private readonly ILogger<ThreadCountService> _logger;
        private Timer _timer;
        private ConcurrentQueue<(long, int)> _threadCounts;

        public ThreadCountService(ILogger<ThreadCountService> logger)
        {
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _threadCounts = new ConcurrentQueue<(long, int)>();
            _timer = new Timer(CountThreads, _threadCounts, TimeSpan.Zero, TimeSpan.FromMilliseconds(500));
            _logger.LogCritical("hi");
            return Task.CompletedTask;
        }

        private static void CountThreads(object state)
        {
            var threadCounts = (ConcurrentQueue<(long, int)>) state;
            var threadCount = Process.GetCurrentProcess().Threads.Count;
            threadCounts.Enqueue((DateTimeOffset.Now.ToUnixTimeSeconds(), threadCount));
        }


        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogCritical("{}", _threadCounts.Count);
            _timer?.Change(Timeout.Infinite, 0);
            await using var file = new StreamWriter("./thread_counts.csv");
            foreach (var timeCountPair in _threadCounts)
            {
                await file.WriteLineAsync($"{timeCountPair.Item1},{timeCountPair.Item2}");
            }
        }

        public void Dispose()
        {
            _timer?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
