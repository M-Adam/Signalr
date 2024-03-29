﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SignalrCommon;

namespace SignalrEmpty.Controllers
{
    public class ExampleController : ControllerBase
    {
        private readonly IHubContext<BritenetChatHub> _hubContext;

        public ExampleController(IHubContext<BritenetChatHub> hubContext)
        {
            _hubContext = hubContext;
        }

        [HttpGet("send/{message}")]
        public async Task<IActionResult> HubTest([FromRoute] string message)
        {
            await _hubContext.Clients.All.SendAsync(ChatMethods.Text, nameof(ExampleController), message);
            return Ok(message);
        }
    }
}
