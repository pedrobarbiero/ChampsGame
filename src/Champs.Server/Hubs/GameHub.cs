using Champs.Shared;
using Microsoft.AspNetCore.SignalR;

namespace Champs.Server;

public class GameHub : Hub
{
    public GameHub()
    {
        game.OnFruitAdded += async (fruitId) =>
        {
            Console.WriteLine($"Fruit added: {fruitId}");
            // await Broadcast(game);
        };
    }
    private static readonly Game game = new();

    public override async Task OnConnectedAsync()
    {
        await base.OnConnectedAsync();
        game.AddNewPlayer(Context.ConnectionId);
        await Broadcast(game);
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await base.OnDisconnectedAsync(exception);
        game.RemovePlayer(Context.ConnectionId);
        await Broadcast(game);
    }

    public async Task MovePlayer(string playerId, Direction direction)
    {
        game.MovePlayer(playerId, direction);
        await Broadcast(game);
    }

    public async Task Broadcast(StateDto state)
    {
        await Clients.All.SendAsync("Broadcast", state);
    }
}
