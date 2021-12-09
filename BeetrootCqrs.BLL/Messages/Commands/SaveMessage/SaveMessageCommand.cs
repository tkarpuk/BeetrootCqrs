using System;
using MediatR;

namespace BeetrootCqrs.BLL.Messages.Commands.SaveMessage
{
    public class SaveMessageCommand : IRequest<Guid>
    {
        public string Text { get; set; }
        public string IpAddress { get; set; }
    }
}
