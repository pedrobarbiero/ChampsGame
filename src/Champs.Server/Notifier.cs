using Champs.Shared;
using Microsoft.AspNetCore.SignalR;

namespace Champs.Server;

public class Notifier : INotifier
{
    private readonly IHubContext<GameHub> _hubContext;
    public Notifier(IHubContext<GameHub> hubContext)
    {
        _hubContext = hubContext;
    }
    public void Broadcast(StateDto state)
    {
        _hubContext.Clients.All.SendAsync("Broadcast", state);
    }
}
