
using GdCore.ThirdParty.TinyMessenger;

namespace GdCore.Services;

public class HermesSubscriber_Disposable : IHermesSubscriber, IDisposable
{
    public List<TinyMessageSubscriptionToken> HermesTokens { get; } = new();

    public void Dispose()
    {
        foreach (TinyMessageSubscriptionToken token in HermesTokens)
            token.Dispose();
    }
}
