using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using StackExchange.Redis;

namespace EShop.Application
{
    public class RedisCleanupService : IHostedService
    {
        public readonly IServer _server;

        public RedisCleanupService(IConnectionMultiplexer connectionMultiplexer)
        {
            _server = connectionMultiplexer.GetServer("localhost", 6379);

        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _server.ExecuteAsync("FLUSHALL");
        }
    }
}
