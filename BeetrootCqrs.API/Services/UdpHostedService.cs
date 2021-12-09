using BeetrootCqrs.BLL.Interfaces;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace BeetrootCqrs.API.Services
{
    public class UdpHostedService : BackgroundService
    {
        private readonly IUdpReceiveService _receiveService;

        public UdpHostedService(IUdpReceiveService receiveService)
        {
            _receiveService = receiveService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _receiveService.ReceiveMessageAsync(stoppingToken);
        }
    }
}
