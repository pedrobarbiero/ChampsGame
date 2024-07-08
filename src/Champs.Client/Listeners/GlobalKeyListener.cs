namespace Champs.Client;

using System.Threading.Tasks;
using Microsoft.JSInterop;

public static class GlobalKeyListener
{
    public static List<Func<string, Task>> KeyDownObservers = [];

    [JSInvokable]
    public static async Task OnKeyDown(string key)
    {
        foreach (var observer in KeyDownObservers)
        {
            await observer.Invoke(key);
        }
    }
}
