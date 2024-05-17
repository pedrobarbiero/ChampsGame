namespace Champs.Shared;

public class Player
{
    public required string Id { get; set; }
    public required string Name { get; set; }
    public required int X { get; set; }
    public required int Y { get; set; }
    public ushort Score { get; set; } = 0;

    public (bool, string) IsCollidingWith(IEnumerable<Fruit> fruits)
    {
        foreach (var fruit in fruits)
        {
            if (X == fruit.X && Y == fruit.Y)
            {
                return (true, fruit.Id);
            }
        }
        return (false, "");
    }
}
