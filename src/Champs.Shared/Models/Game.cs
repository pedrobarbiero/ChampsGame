using System.Collections.ObjectModel;

namespace Champs.Shared;

public class Game
{
    private readonly Dictionary<string, Player> _players = [];
    private readonly Dictionary<string, Fruit> _fruits = [];
    public ReadOnlyDictionary<string, Player> Players => _players.AsReadOnly();
    public ReadOnlyDictionary<string, Fruit> Fruits => _fruits.AsReadOnly();
    public Board Board { get; init; } = new Board() { Height = 10, Width = 10 };

    public Game()
    {
        StartCyclingFruits();
    }


    public static implicit operator StateDto(Game game)
    {
        return new StateDto
        {
            Board = game.Board,
            Players = game._players,
            Fruits = game._fruits
        };
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

    public void StartCyclingFruits()
    {
        Task.Run(async () =>
        {
            var timer = new PeriodicTimer(TimeSpan.FromSeconds(3));
            while (await timer.WaitForNextTickAsync())
            {
                var id = Guid.NewGuid().ToString();
                _fruits.Add(id, new Fruit
                {
                    Id = id,
                    X = new Random().Next(0, Board.Width),
                    Y = new Random().Next(0, Board.Height)
                });
                OnFruitAdded?.Invoke(id);
            }
        });
    }

    public Action<string> OnFruitAdded;



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

public enum Direction
{
    Up,
    Down,
    Left,
    Right
}