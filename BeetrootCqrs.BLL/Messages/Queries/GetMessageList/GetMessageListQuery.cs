using System;
using System.Collections.Generic;
using BeetrootCqrs.BLL.Dto;
using MediatR;

namespace BeetrootCqrs.BLL.Messages.Queries.GetMessageList
{
    public class GetMessageListQuery : IRequest<IEnumerable<MessageDto>>
    {
        public string IpAddress { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
    }
}
