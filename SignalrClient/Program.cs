using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.SignalR.Client;
using SignalrCommon;

namespace SignalrClient
{
    class Program
    {
        //const string ConnectionString = "https://localhost:44300/chat";
        const string ConnectionString = "https://signalrempty.azurewebsites.net/chat";

        static HubConnection _hubConnection;
        
        static async Task Main(string[] args)
        {
            _hubConnection = new HubConnectionBuilder()
                .WithUrl(ConnectionString)
                .Build();

            var chatMethodHandler =
                _hubConnection.On<string, string>(ChatMethods.Text, PrintChatMessage);

            await _hubConnection.StartAsync();

            await Chat();

            chatMethodHandler.Dispose();
            await _hubConnection.StopAsync();
        }

        static void PrintChatMessage(string sender, string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"[{DateTime.Now.ToShortTimeString()}] {sender}: {message}");
            Console.ResetColor();
        }

        static async Task Chat()
        {
            Console.Write("Username: ");
            var username = Console.ReadLine();
            string input;

            do
            {
                input = Console.ReadLine();
                ClearCurrentConsoleLine();

                if (string.IsNullOrWhiteSpace(input))
                    continue;

                await _hubConnection.InvokeAsync(ChatMethods.Text, username, input);

            } while (input?.Equals("exit") != true);
        }

        static void ClearCurrentConsoleLine()
        {
            var currentLineCursor = Console.CursorTop - 1;
            Console.SetCursorPosition(0, Console.CursorTop - 1);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, currentLineCursor);
        }
    }
}
