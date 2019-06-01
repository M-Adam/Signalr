using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;

namespace SignalrClient
{
    class Program
    {
        static HubConnection _hubConnection;
        const string HubChatMethod = "sendToAll";
        const string ConnectionString = "https://localhost:44300/chat";
        static IDisposable _handler;

        static async Task Main(string[] args)
        {
            _hubConnection = new HubConnectionBuilder()
                .WithUrl(ConnectionString)
                .Build();

            _hubConnection.Closed += HubConnectionOnClosed;
            _handler = _hubConnection.On<string, string>("sendToAll", PrintMessage);

            await _hubConnection.StartAsync();
            Console.WriteLine($"Connected to {ConnectionString}.");

            await EnterChat();

            _handler?.Dispose();
            await _hubConnection.StopAsync();
            await _hubConnection.DisposeAsync();
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
                
                await _hubConnection.InvokeAsync(HubChatMethod, nickname, input);
            } while (input?.Equals("exit") != true);
        }

        static void PrintMessage(string sender, string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"[{DateTime.Now.ToShortTimeString()}] {sender}: {message}");
            Console.ResetColor();
        }

        static void ClearCurrentConsoleLine()
        {
            var currentLineCursor = Console.CursorTop-1;
            Console.SetCursorPosition(0, Console.CursorTop-1);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, currentLineCursor);
        }

        static async Task HubConnectionOnClosed(Exception arg)
        {
            Console.Error.WriteLine(arg);
            await _hubConnection.StartAsync();
        }
    }
}
