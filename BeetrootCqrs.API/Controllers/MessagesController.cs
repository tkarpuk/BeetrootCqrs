using BeetrootCqrs.BLL.Dto;
using BeetrootCqrs.BLL.Messages.Queries.GetMessageList;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BeetrootCqrs.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly IMediator _mediatr;
        private readonly ILogger<MessagesController> _logger;

        public MessagesController(IMediator mediatr, ILogger<MessagesController> logger)
        {
            _mediatr = mediatr;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<List<MessageDto>>> Get(string ipAddress,
            DateTime dateStart, DateTime dateEnd)
        {
            _logger.LogDebug($"Request with parameterstring: {Request.QueryString}");

            var query = new GetMessageListQuery()
            {
                IpAddress = ipAddress,
                DateStart = dateStart,
                DateEnd = dateEnd
            };

            var list = await _mediatr.Send(query);
            return Ok(list);
        }
    }
}
