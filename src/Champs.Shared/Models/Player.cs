namespace Champs.Shared;

public class Player
{
    public required string Name { get; set; }
    public required int X { get; set; }
    public required int Y { get; set; }
    public ushort Score { get; set; } = 0;
}
