using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using SignalrCommon;

namespace SignalrBenchmark
{
    class Program
    {
        private const int NumberOfClients = 600;
        const string ConnectionStringEmbedded = "https://signalrempty.azurewebsites.net/chat";
        const string ConnectionStringService = "https://signalrazure.azurewebsites.net/chat";

        static async Task Main(string[] args)
        {
            //var str = Console.ReadKey().KeyChar == '1' ? ConnectionStringEmbedded : ConnectionStringService;

            var hubConnections = new List<HubConnection>(NumberOfClients);
            for (var i = 0; i < NumberOfClients; i++)
            {
                hubConnections.Add(new HubConnectionBuilder().WithUrl(i % 2 == 0 ? ConnectionStringEmbedded : ConnectionStringService, x=>x.CloseTimeout = TimeSpan.FromMinutes(5)).Build());
            }

            var cts = new CancellationTokenSource();
            var tasks = new List<Task>();
            cts.Token.Register(() =>
            {
                hubConnections.ForEach(async x =>
                {
                    //await x.StopAsync(CancellationToken.None);
                    await x.DisposeAsync();
                });
                tasks.ForEach(x=>x?.Dispose());
                Environment.Exit(1);
            });
            
            Console.CancelKeyPress += (s, ev) =>
            {
                Console.WriteLine("Ctrl+C pressed");
                ev.Cancel = true;
                cts.Cancel();
            };

            hubConnections.ForEach(async x =>
            {
                x.Closed += exception =>
                {
                    Console.Error.WriteLine(exception);
                    return x.StartAsync(cts.Token);
                };
                //await x.StartAsync(cts.Token);
            });

            var j = 0;
            hubConnections.ForEach(x =>
                {
                    tasks.Add(Task.Factory.StartNew(async () =>
                    {
                        Console.WriteLine(++j + " started.");
                        await Spam(cts.Token, x, j);
                    }, cts.Token));
                });

            await Task.Delay(TimeSpan.FromHours(1), CancellationToken.None);
        }

        private static async Task Spam(CancellationToken c, HubConnection connection, int j)
        {
            while (!c.IsCancellationRequested)
            {
                var a = Guid.NewGuid().ToString();
                var b = Guid.NewGuid().ToString();
                Console.WriteLine(j + "\t:" + a + b);
                try
                {
                    await connection.StartAsync(c);
                    //await Task.Delay(50, c);
                    await connection.InvokeAsync(ChatMethods.Text, a, b, c);
                    await connection.StopAsync(c);
                }
                catch { }
                
            }
            Console.WriteLine(j + " out of loop");
        }
    }

        
}
