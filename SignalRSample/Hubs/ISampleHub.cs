using System.Threading.Tasks;

namespace SignalRSample.Hubs
{
    public interface ISampleHub
    {
        Task SendMessage(string message);
    }
}