using System.Threading;
using System.Threading.Tasks;

namespace BeetrootCqrs.BLL.Interfaces
{
    public interface IUdpReceiveService
    {
        Task ReceiveMessageAsync(CancellationToken stoppingToken);
    }
}
