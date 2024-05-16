using Champs.Shared;
using Microsoft.AspNetCore.SignalR;

namespace Champs.Server;

public class GameHub : Hub
{
    private static readonly Game game = new();
    public async Task ConnectPlayer(string playerId)
    {
        Console.WriteLine($"ConnectPlayer {playerId}");
        game.AddPlayer(playerId, new Player { Name = playerId, X = 1, Y = 1 }); //Todo: add random x and y
        await Broadcast(game.State);
    }

    public async Task MovePlayer(string playerId, Direction direction)
    {
        Console.WriteLine($"MovePlayer {playerId} {direction}");
        game.MovePlayer(playerId, direction);
        await Broadcast(game.State);
    }

    public async Task Broadcast(GameState gameState)
    {
        await Clients.All.SendAsync("Broadcast", gameState);
        Console.WriteLine($"Broadcasted state: {gameState}");
    }
}
