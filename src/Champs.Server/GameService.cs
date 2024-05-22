using System.Collections.ObjectModel;
using System.Timers;
using Champs.Shared;

namespace Champs.Server;

public class GameService
{
    private readonly Dictionary<string, Player> _players = [];
    private readonly Dictionary<string, Fruit> _fruits = [];
    private readonly INotifier _notifier;
    public ReadOnlyDictionary<string, Player> Players => _players.AsReadOnly();
    public ReadOnlyDictionary<string, Fruit> Fruits => _fruits.AsReadOnly();
    public Board Board { get; init; } = new Board() { Height = 10, Width = 10 };
    private readonly System.Timers.Timer _timer;

    public GameService(INotifier notifier)
    {
        _timer = new System.Timers.Timer();
        _notifier = notifier;
        StartCycling(3000);
    }

    private void StartCycling(double interval)
    {
        _timer.Interval = interval;
        _timer.Elapsed += GenerateFruit;
        _timer.Start();
    }

    private void StopCycling()
    {
        _timer.Stop();
        _timer.Elapsed -= GenerateFruit;
    }

    ~GameService()
    {
        StopCycling();
    }

    public StateDto State => new()
    {
        Board = Board,
        Players = _players,
        Fruits = _fruits
    };


    private void GenerateFruit(object? sender, ElapsedEventArgs e)
    {
        var fruit = new Fruit
        {
            Id = Guid.NewGuid().ToString(),
            X = new Random().Next(0, Board.Width),
            Y = new Random().Next(0, Board.Height)
        };
        _fruits.Add(fruit.Id, fruit);
        _notifier.Broadcast(State);
    }

    public void AddNewPlayer(string playerId)
    {
        _players.Add(playerId, new Player
        {
            Id = playerId,
            Name = playerId,
            X = new Random().Next(0, Board.Width),
            Y = new Random().Next(0, Board.Height)
        });
    }

    public void RemovePlayer(string playerId)
    {
        _players.Remove(playerId);
    }

    public void MovePlayer(string playerId, Direction direction)
    {
        var player = _players[playerId];
        if (direction == Direction.Up && player.Y > 0)
        {
            player.Y--;
        }
        else if (direction == Direction.Down && player.Y < Board.Height - 1)
        {
            player.Y++;
        }
        else if (direction == Direction.Left && player.X > 0)
        {
            player.X--;
        }
        else if (direction == Direction.Right && player.X < Board.Width - 1)
        {
            player.X++;
        }
        PickFruitIfCollide(player);
    }

    private void PickFruitIfCollide(Player player)
    {
        var (colliding, fruitId) = player.IsCollidingWith(_fruits.Values);
        if (colliding)
            PickFruit(player, fruitId);
    }

    private void PickFruit(Player player, string fruitId)
    {
        _fruits.Remove(fruitId);
        player.Score++;
    }
}