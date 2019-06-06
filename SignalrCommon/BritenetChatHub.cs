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

        [HubMethodName(ChatMethods.Object)]
        public async Task<Guid> SendObject(string sender, ExampleEntity obj)
        {
            await Clients.AllExcept(Context.ConnectionId).SendAsync(ChatMethods.Object, sender, obj);
            return Guid.NewGuid();
        }
    }
}
