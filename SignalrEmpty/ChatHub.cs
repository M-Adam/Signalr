using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace SignalrEmpty
{
    public class ChatHub : Hub
    {
        public async Task SendToAll(string sender, string message)
        {
            await Clients.All.SendAsync("sendToAll", sender, message);
        }
    }
}
