using System;
using System.Threading;
using System.Threading.Tasks;
using BeetrootCqrs.DAL.Entities;
using BeetrootCqrs.DAL.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BeetrootCqrs.BLL.Messages.Commands.SaveMessage
{
    public class SaveMessageCommandHandler : IRequestHandler<SaveMessageCommand, Guid>
    {
        private readonly IAppDbContext _dbContext;
        public SaveMessageCommandHandler(IAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Guid> Handle(SaveMessageCommand request, CancellationToken cancellationToken)
        {
            Address address = await GetAddressAsync(request.IpAddress, cancellationToken);

            var message = new Message()
            {
                Id = Guid.NewGuid(),
                Text = request.Text,
                Date = DateTime.UtcNow,
                IpAddress = address
            };

            _dbContext.Messages.Add(message);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return message.Id;
        }

        private async Task<Address> GetAddressAsync(string IpAddress, CancellationToken cancellationToken)
        {
            Address address = await _dbContext.Addresses.FirstOrDefaultAsync(a => a.IpAddress == IpAddress, cancellationToken);
            if (address == null)
            {
                address = new Address()
                {
                    Id = Guid.NewGuid(),
                    IpAddress = IpAddress
                };

                _dbContext.Addresses.Add(address);
                await _dbContext.SaveChangesAsync(cancellationToken);
            }

            return address;
        }
    }
}
