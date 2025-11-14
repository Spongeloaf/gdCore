using GdCore.ThirdParty.TinyIoC;
using GdCore.ThirdParty.TinyMessenger;

namespace GdCore.Services;

public interface IHermesSubscriber
{
    List<TinyMessageSubscriptionToken> HermesTokens { get; }
}

/// <summary>
/// Hermes is an easy messenger service that works using a publish/subscribe model.
/// Anyone can publish messages.<br/>
///
/// To subscribe to messages, make your class inherit from the IHermesSubscriber
/// interface and call Hermes.Subscribe&lt;MessageType&gt;(this, ...)
/// inside your constructor.
/// </summary>
public static class Hermes
{
    // Built on: https://github.com/grumpydev/TinyMessenger

    private static ITinyMessengerHub hub = TinyIoCContainer.Current.Resolve<ITinyMessengerHub>();

    /// <summary>
    /// Creates a subscriber as a child of the given node and subscribes to messages.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="node"></param>
    /// <param name="action"></param>
    public static void CreateAndSubscribe<T>(Node node, Action<T> action) where T : class, ITinyMessage
    {
        var hermesSub = new HermesSubscriberNode();
        node.AddChild(hermesSub);
        Subscribe<T>(hermesSub, action);
    }

    public static void Subscribe<T>(Node node, Action<T> action) where T : class, ITinyMessage
    {
        if (node is not IHermesSubscriber subscriber)
            return;

        // Ensures automatic cleanup of subscription when the sub node is freed or removed from the tree.
        node.TreeExiting += () =>
        {
            if (node is IHermesSubscriber sub)
            {
                sub.HermesTokens.ForEach(t => t.Dispose());
                sub.HermesTokens.Clear();
            }
        };

        subscriber.HermesTokens.Add(hub.Subscribe(action));
    }

    public static void Subscribe<T>(HermesSubscriber_Disposable subscriber, Action<T> action) where T : class, ITinyMessage
    {
        subscriber.HermesTokens.Add(hub.Subscribe(action));
    }

    public static TinyMessageSubscriptionToken Subscribe<T>(IHermesSubscriber subscriber, Action<T> action, Func<T, bool> predicate) where T : class, ITinyMessage
    {
        return hub.Subscribe(action, predicate);
    }

    public static TinyMessageSubscriptionToken Subscribe<T>(IHermesSubscriber subscriber, Action<T> action, bool useWekRefInsteadOfString) where T : class, ITinyMessage
    {
        return hub.Subscribe(action, useWekRefInsteadOfString);
    }

    public static TinyMessageSubscriptionToken Subscribe<T>(IHermesSubscriber subscriber, Action<T> action, bool useWekRefInsteadOfString, Func<T, bool> predicate) where T : class, ITinyMessage
    {
        return hub.Subscribe(action, predicate, useWekRefInsteadOfString);
    }

    public static void Publish<T>(T message) where T : class, ITinyMessage
    {
        hub.Publish<T>(message);
    }

    public static void Unsubscribe<T>(IHermesSubscriber subscriber, TinyMessageSubscriptionToken token) where T : class, ITinyMessage
    {
        subscriber.HermesTokens.RemoveAll(t => t == token);
        hub.Unsubscribe<T>(token);
    }
}

// ReSharper restore InconsistentNaming
