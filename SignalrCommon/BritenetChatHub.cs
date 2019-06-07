using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace SignalrCommon
{
    public class BritenetChatHub : Hub
    {
        [HubMethodName(ChatMethods.Text)]
        public async Task SendChat(string sender, string message)
        {
            await Clients.All.SendAsync(ChatMethods.Text, sender, message);
        }
    }
}
