using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using SignalrCommon;
using static SignalrClient.Helpers;

namespace SignalrClient
{
    class Program
    {
        const string ConnectionString = "https://localhost:44300/chat";

        private static readonly List<IDisposable> Handlers = new List<IDisposable>();
        static HubConnection _hubConnection;
        

        static async Task Main(string[] args)
        {
            _hubConnection = new HubConnectionBuilder()
                .WithUrl(ConnectionString)
                .Build();
            
            _hubConnection.Closed += HubConnectionOnClosed;

            var chatMethodHandler = 
                _hubConnection.On<string, string>(ChatMethods.Text, PrintChatMessage);
            
            var objectMethodHandler = 
                _hubConnection.On<string, ExampleEntity>(ChatMethods.Object, PrintObjectMessage);

            Handlers.Add(chatMethodHandler);
            Handlers.Add(objectMethodHandler);

            await _hubConnection.StartAsync();
            Console.WriteLine($"Connected to {ConnectionString}.");

            await EnterChat();

            Handlers.ForEach(x => x.Dispose());
            await _hubConnection.StopAsync();
        }

        static async Task EnterChat()
        {
            Console.WriteLine("Enter your nickname: ");
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

        static async Task HubConnectionOnClosed(Exception arg)
        {
            Console.Error.WriteLine(arg);
            await _hubConnection.StartAsync();
        }
    }
}
