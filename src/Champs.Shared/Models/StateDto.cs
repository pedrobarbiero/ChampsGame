namespace Champs.Shared;

public record StateDto
{
    public required Dictionary<string, Player> Players { get; init; }
    public required Dictionary<string, Fruit> Fruits { get; init; }
    public required List<Fruit> FakeFruits { get; init; }
    public required Board Board { get; init; }
}
