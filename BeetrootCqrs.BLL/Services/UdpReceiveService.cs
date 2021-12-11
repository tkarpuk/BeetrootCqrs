using BeetrootCqrs.BLL.Configurations;
using BeetrootCqrs.BLL.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using BeetrootCqrs.BLL.Messages.Commands.SaveMessage;

namespace BeetrootCqrs.BLL.Services
{
    public class UdpReceiveService : IUdpReceiveService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger<UdpReceiveService> _logger;
        private readonly UdpConfiguration _configuration;

        public UdpReceiveService(IServiceScopeFactory serviceScopeFactory, IOptions<UdpConfiguration> udpConfiguration, 
            ILogger<UdpReceiveService> logger)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
            _configuration = udpConfiguration.Value;

            _logger.LogInformation($"Created UdpReceiveService. Port: {_configuration.PortUdp}");
        }

        private bool IsNotProperlyMessage(string message)
        {
            return !((message.Length > _configuration.SecretKey.Length) && message.Contains(_configuration.SecretKey));
        }

        private string ClearMessageText(string message)
        {
            return message.Replace(_configuration.SecretKey, "");
        }

        private async Task SaveMessageAsync(string text, string address, CancellationToken cancellationToken)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            IMediator mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            SaveMessageCommand command = new SaveMessageCommand()
            {
                IpAddress = address,
                Text = text
            };
            Guid messageId = await mediator.Send(command, cancellationToken);

            _logger.LogDebug($"ID saved message in DB: {messageId}");
        }

        public async Task ReceiveMessageAsync(CancellationToken stoppingToken)
        {
            using var receiver = new UdpClient(_configuration.PortUdp);
            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    var udpReceiveResult = await receiver.ReceiveAsync();
                    _logger.LogDebug($"Get message from IP: {udpReceiveResult.RemoteEndPoint.Address}");

                    string text = Encoding.Unicode.GetString(udpReceiveResult.Buffer);
                    if (IsNotProperlyMessage(text))
                    {
                        _logger.LogError($"Message is empty or doesn't contain Secret Key");
                        continue;
                    }

                    text = ClearMessageText(text);
                    string address = udpReceiveResult.RemoteEndPoint.Address.ToString();

                    await SaveMessageAsync(text, address, stoppingToken);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in ReceiveMessageAsync: {ex.Message}");
            }
            finally
            {
                receiver.Close();
            }
        }
    }
}
