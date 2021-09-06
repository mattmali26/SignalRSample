using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ConsoleApp
{
    internal class Program
    {
        private async static Task Main(string[] args)
        {
            Console.WriteLine("SignalR sample test: press esc to exit, press S to send a message");

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
                .AddJsonFile("appsettings.json", false, true)
                .Build();

            var hubAddress = configuration.GetSection("SignalRHub").Value;

            var connection = new HubConnectionBuilder().WithUrl(hubAddress).Build();

            connection.Closed += async (error) =>
            {
                Console.WriteLine(error);
                await Task.Delay(3000);
                await connection.StartAsync();
            };

            connection.On<string>("SendMessage", (message) =>
            {
                Console.WriteLine($"New message received at {DateTime.Now}: {message}");
            });

            await connection.StartAsync();

            while (Console.ReadKey(true).Key != ConsoleKey.Escape)
            {
                if (Console.ReadKey().Key == ConsoleKey.S)
                {
                    Console.Write("Insert new message: ");
                    var message = Console.ReadLine();
                    await connection.InvokeAsync("SendMessage", message);
                }
            }
        }
    }
}