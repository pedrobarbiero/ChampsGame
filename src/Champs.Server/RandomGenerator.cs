namespace Champs.Server;

public class RandomGenerator : IRandomGenerator
{
    public int Generate(int min, int max) => new Random().Next(min, max);
}
