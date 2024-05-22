using Champs.Shared;
using Microsoft.AspNetCore.SignalR;

namespace Champs.Server;

public class GameHub(GameService game) : Hub
{
    private readonly GameService _gameService = game;

    public override async Task OnConnectedAsync()
    {
        await base.OnConnectedAsync();
        _gameService.AddNewPlayer(Context.ConnectionId);
        await Broadcast(_gameService.State);
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await base.OnDisconnectedAsync(exception);
        _gameService.RemovePlayer(Context.ConnectionId);
        await Broadcast(_gameService.State);
    }

    public async Task MovePlayer(string playerId, Direction direction)
    {
        _gameService.MovePlayer(playerId, direction);
        await Broadcast(_gameService.State);
    }

    public async Task Broadcast(StateDto state)
    {
        await Clients.All.SendAsync("Broadcast", state);
    }
}
