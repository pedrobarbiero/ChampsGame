using Champs.Server;
using Champs.Shared;
using Microsoft.Extensions.Time.Testing;

namespace ChampsGame.Tests;


public class GameServiceTest
{
    private readonly GameService _gameService;
    private readonly FakeTimeProvider _fakeTimer;
    public GameServiceTest()
    {
        var notifier = A.Fake<INotifier>();

        var randomGenerator = A.Fake<IRandomGenerator>();
        A.CallTo(() => randomGenerator.Generate(A<int>._, A<int>._)).Returns(5);

        _fakeTimer = new FakeTimeProvider(startDateTime: DateTimeOffset.UtcNow);

        _gameService = new GameService(notifier, randomGenerator, _fakeTimer);
    }
    [Fact]
    public void AddNewPlayer_AddsPlayer()
    {
        _gameService.AddNewPlayer("1");
        Assert.Contains("1", _gameService.State.Players.Keys);
    }

    [Fact]
    public void RemovePlayer_RemovesPlayer()
    {
        _gameService.AddNewPlayer("1");
        _gameService.AddNewPlayer("2");
        _gameService.RemovePlayer("1");

        Assert.Contains("2", _gameService.State.Players.Keys);
        Assert.DoesNotContain("1", _gameService.State.Players.Keys);
        Assert.Single(_gameService.State.Players);
    }

    [Fact]
    public void MovePlayer_MovesPlayerUpAndLeft()
    {
        _gameService.AddNewPlayer("1");
        _gameService.MovePlayer("1", Direction.Up);
        _gameService.MovePlayer("1", Direction.Left);
        Assert.Equal(4, _gameService.State.Players["1"].X);
        Assert.Equal(4, _gameService.State.Players["1"].Y);
    }

    [Fact]
    public void MovePlayer_MovesPlayerDownAndRight()
    {
        _gameService.AddNewPlayer("1");
        _gameService.MovePlayer("1", Direction.Down);
        _gameService.MovePlayer("1", Direction.Right);
        Assert.Equal(6, _gameService.State.Players["1"].X);
        Assert.Equal(6, _gameService.State.Players["1"].Y);
    }

    [Fact]
    public void GenerateFruit_AddsFruitAfter3Seconds()
    {
        Assert.Empty(_gameService.State.Fruits);
        _fakeTimer.Advance(TimeSpan.FromSeconds(3));
        Assert.Single(_gameService.State.Fruits);
    }

    [Fact]
    public void GenerateFruit_AddsFruitEvery3Seconds()
    {
        Assert.Empty(_gameService.State.Fruits);
        _fakeTimer.Advance(TimeSpan.FromSeconds(3));
        Assert.Single(_gameService.State.Fruits);
        _fakeTimer.Advance(TimeSpan.FromSeconds(3));
        Assert.Equal(2, _gameService.State.Fruits.Count);
    }

    [Fact]
    public void PlayerEatsFruit_RemovesFruitAndIncreaseScore()
    {
        _gameService.AddNewPlayer("1");
        _gameService.MovePlayer("1", Direction.Up);
        _fakeTimer.Advance(TimeSpan.FromSeconds(3));
        _gameService.MovePlayer("1", Direction.Down);

        Assert.Empty(_gameService.State.Fruits);
        Assert.Equal(1, _gameService.State.Players["1"].Score);
    }

}
