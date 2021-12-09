using System;

namespace BeetrootCqrs.BLL.Dto
{
    public class MessageDto
    {
        public string IpAddress { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
    }
}
