namespace Champs.Client;

public class GameState
{
    public GameState(IDictionary<string, Player> players, IDictionary<string, Fruit> fruits, Board board)
    {
        _players = players.ToDictionary();
        _fruits = fruits.ToDictionary();
        Board = board;
    }

    private Dictionary<string, Player> _players;
    private Dictionary<string, Fruit> _fruits;
    public Board Board { get; init ; }
    public IReadOnlyDictionary<string, Player> Players => _players.AsReadOnly();
    public IReadOnlyDictionary<string, Fruit> Fruits => _fruits.AsReadOnly();
    public void RemoveFruit(string fruitId) => _fruits.Remove(fruitId);
}