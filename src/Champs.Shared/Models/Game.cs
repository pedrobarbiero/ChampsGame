namespace Champs.Shared;

public class Game
{
    public Game()
    {
        Console.WriteLine("Game created");
        State = new GameState();
    }
    public GameState State { get; private set; }
    public void AddPlayer(string playerId, Player player)
    {
        State.AddPlayer(playerId, player);
    }
    public void MovePlayer(string playerId, Direction direction)
    {
        var player = State.Players[playerId];
        Console.WriteLine($"MovePlayer {playerId} {direction}");
        if (direction == Direction.Up && player.Y > 0)
        {
            player.Y--;
        }
        else if (direction == Direction.Down && player.Y < State.Board.Height - 1)
        {
            player.Y++;
        }
        else if (direction == Direction.Left && player.X > 0)
        {
            player.X--;
        }
        else if (direction == Direction.Right && player.X < State.Board.Width - 1)
        {
            player.X++;
        }
        PickFruitIfCollide(playerId);
    }

    private void PickFruitIfCollide(string playerId)
    {
        var player = State.Players[playerId];
        foreach (KeyValuePair<string, Fruit> entry in State.Fruits)
        {
            if (PlayerAndFruitCollided(player, entry.Value))
            {
                State.RemoveFruit(entry.Key);
                break;
            }
        }
    }

    private bool PlayerAndFruitCollided(Player player, Fruit fruit) =>
        player.X == fruit.X && player.Y == fruit.Y;

}

public enum Direction
{
    Up,
    Down,
    Left,
    Right
}