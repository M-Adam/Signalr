using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.SignalR.Client;
using SignalrCommon;
using static SignalrClient.Helpers;

namespace SignalrClient
{
    class Program
    {
        const string ConnectionString = "https://signalrempty.azurewebsites.net/chat";

        static List<IDisposable> _handlers;
        static HubConnection _hubConnection;
        
        static async Task Main(string[] args)
        {
            _hubConnection = new HubConnectionBuilder()
                .WithUrl(ConnectionString)
                .Build();

            var chatMethodHandler =
                _hubConnection.On<string, string>(ChatMethods.Text, PrintChatMessage);

            var objectMethodHandler =
                _hubConnection.On<string, ExampleEntity>(ChatMethods.Object, PrintObjectMessage);

            _handlers = new List<IDisposable>
            {
                chatMethodHandler, objectMethodHandler
            };

            await _hubConnection.StartAsync();

            await EnterChat();

            _handlers.ForEach(x => x.Dispose());
            await _hubConnection.StopAsync();
        }

        static async Task EnterChat()
        {
            Console.Write("Nickname: ");
            var nickname = Console.ReadLine();
            string input;

            do
            {
                input = Console.ReadLine();
                ClearCurrentConsoleLine();

                if (string.IsNullOrWhiteSpace(input))
                    continue;

                if (input.Equals("object"))
                {
                    var result = await _hubConnection.InvokeAsync<Guid>(ChatMethods.Object, nickname, new ExampleEntity
                    {
                        Data = "some data"
                    });
                    Console.WriteLine("Sending object result: " + result);
                }
                else
                {
                    await _hubConnection.InvokeAsync(ChatMethods.Text, nickname, input);
                }
            } while (input?.Equals("exit") != true);
        }
    }
}
