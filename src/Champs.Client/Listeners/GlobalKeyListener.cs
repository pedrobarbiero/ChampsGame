namespace Champs.Client;

using Microsoft.JSInterop;
using System.Threading.Tasks;

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