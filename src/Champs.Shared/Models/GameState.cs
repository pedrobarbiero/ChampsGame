namespace Champs.Shared;

public class GameState
{    
    // private readonly Dictionary<string, Player> _players = [];
    // private readonly Dictionary<string, Fruit> _fruits = [];
    public Board Board { get; init; } = new Board() { Height = 10, Width = 10 };
    public Dictionary<string, Player> Players { get; set; } = []; // Todo: refactor to private and use DTO
    public Dictionary<string, Fruit> Fruits { get; set; } = [];
    public void RemoveFruit(string fruitId) => Fruits.Remove(fruitId);
    public void AddPlayer(string Id, Player player) => Players.Add(Id, player);
    public string Random = "123123";

    public override string ToString()
    {
        return $"Players: {Players.Count}, Fruits: {Fruits.Count}, Board: {Board.Width}x{Board.Height}, Random: {Random}";
    }
}