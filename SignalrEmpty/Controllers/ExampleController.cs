using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SignalrCommon;
using SignalrEmpty.Hubs;

namespace SignalrEmpty.Controllers
{
    public class ExampleController : ControllerBase
    {
        private readonly IHubContext<BritenetChatHub> _hubContext;

        public ExampleController(IHubContext<BritenetChatHub> hubContext)
        {
            _hubContext = hubContext;
        }

        [HttpGet("{message}")]
        public async Task<IActionResult> HubTest([FromRoute] string message)
        {
            await _hubContext.Clients.All.SendAsync(ChatMethods.Text, nameof(ExampleController), message);
            return Ok(message);
        }

        [HttpGet("exception")]
        public IActionResult Exception()
        {
            throw new Exception();
        }
    }
}
