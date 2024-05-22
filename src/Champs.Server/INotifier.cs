using Champs.Shared;

namespace Champs.Server;

public interface INotifier
{
    void Broadcast(StateDto state);
}
