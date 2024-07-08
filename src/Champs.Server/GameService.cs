using Champs.Shared;

namespace Champs.Server;

public class GameService
{
    private readonly Dictionary<string, Player> _players = [];
    private readonly Dictionary<string, Fruit> _fruits = [];
    private readonly List<Fruit> _fakeFruits = [];
    private readonly INotifier _notifier;
    private readonly IRandomGenerator _randomGenerator;
    public Board Board { get; init; } = new Board() { Height = 10, Width = 10 };
    private readonly ITimer _fruitTimer;
    private readonly ITimer _fakeFruitTimer;
    private const int MAX_FAKE_FRUITS = 3;

    public GameService(
        INotifier notifier,
        IRandomGenerator randomGenerator,
        TimeProvider timerProvider
    )
    {
        _fruitTimer = timerProvider.CreateTimer(
            GenerateFruitCallback,
            null,
            TimeSpan.FromSeconds(3),
            TimeSpan.FromSeconds(3)
        );
        _fakeFruitTimer = timerProvider.CreateTimer(
            GenerateFakeFruitCallback,
            null,
            TimeSpan.FromSeconds(22),
            TimeSpan.FromSeconds(3)
        );
        _notifier = notifier;
        _randomGenerator = randomGenerator;
    }

    private void StopCycling()
    {
        _fruitTimer.Change(TimeSpan.MaxValue, TimeSpan.MaxValue);
        _fakeFruitTimer.Change(TimeSpan.MaxValue, TimeSpan.MaxValue);
    }

    ~GameService()
    {
        StopCycling();
    }

    public StateDto State =>
        new()
        {
            Board = Board,
            Players = _players,
            Fruits = _fruits,
            FakeFruits = _fakeFruits
        };

    void GenerateFakeFruitCallback(object? state)
    {
        for (var i = 0; i < MAX_FAKE_FRUITS; i++)
        {
            _fakeFruits.Add(
                new Fruit
                {
                    Id = Guid.NewGuid().ToString(),
                    X = _randomGenerator.Generate(0, Board.Width),
                    Y = _randomGenerator.Generate(0, Board.Height)
                }
            );
        }
    }

    void GenerateFruitCallback(object? state)
    {
        if (_fakeFruits.Count > 0)
        {
            var index = _randomGenerator.Generate(0, _fakeFruits.Count);
            _fruits.Add(_fakeFruits[index].Id, _fakeFruits[index]);
            _fakeFruits.Clear();
        }
        else
        {
            var fruit = new Fruit
            {
                Id = Guid.NewGuid().ToString(),
                X = _randomGenerator.Generate(0, Board.Width),
                Y = _randomGenerator.Generate(0, Board.Height)
            };
            _fruits.Add(fruit.Id, fruit);
        }
        _notifier.Broadcast(State);
    }

    public void AddNewPlayer(string playerId)
    {
        _players.Add(
            playerId,
            new Player
            {
                Id = playerId,
                Name = playerId,
                X = _randomGenerator.Generate(0, Board.Width),
                Y = _randomGenerator.Generate(0, Board.Height)
            }
        );
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
