using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BeetrootCqrs.BLL.Dto;
using BeetrootCqrs.DAL.Entities;
using BeetrootCqrs.DAL.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BeetrootCqrs.BLL.Messages.Queries.GetMessageList
{
    public class GetMessageListQueryHandler : IRequestHandler<GetMessageListQuery, IEnumerable<MessageDto>>
    {
        private readonly IAppDbContext _dbContext;
        public GetMessageListQueryHandler(IAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        public async Task<IEnumerable<MessageDto>> Handle(GetMessageListQuery request, CancellationToken cancellationToken)
        {
            Func<Message, bool> AddressCondition = CreateAddressCondition(request);
            Func<Message, bool> MessageCondition = CreateMessageCondition(request);

            var listResult = _dbContext.Messages.Include(m => m.IpAddress)
                .Where(AddressCondition)
                .Where(MessageCondition)
                .Select(m => new MessageDto()
                {
                    IpAddress = m.IpAddress.IpAddress,
                    Date = m.Date,
                    Text = m.Text
                });

            return await Task.Run(() => listResult, cancellationToken);
        }

        private static Func<Message, bool> CreateAddressCondition(GetMessageListQuery request)
        {
            if (string.IsNullOrEmpty(request.IpAddress))
                return m => true;

            return m => (m.IpAddress.IpAddress == request.IpAddress);
        }

        private static Func<Message, bool> CreateMessageCondition(GetMessageListQuery request)
        {
            if (request.DateStart != DateTime.MinValue && request.DateEnd != DateTime.MinValue)
                return m => m.Date >= request.DateStart && m.Date <= request.DateEnd;

            if (request.DateStart != DateTime.MinValue && request.DateEnd == DateTime.MinValue)
                return m => m.Date >= request.DateStart;

            if (request.DateStart == DateTime.MinValue && request.DateEnd != DateTime.MinValue)
                return m => m.Date <= request.DateEnd;

            return m => true;
        }
    }
}
