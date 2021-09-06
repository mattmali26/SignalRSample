using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace SignalRSample.Hubs
{
    public class SampleHub : Hub<ISampleHub>
    {
        public async Task SendMessage(string message)
        {
            await Clients.All.SendMessage(message);
        }
    }
}