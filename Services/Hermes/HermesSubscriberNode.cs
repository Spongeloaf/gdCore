using GdCore.ThirdParty.TinyMessenger;

namespace GdCore.Services;

/// <summary>
/// Allows a parent node to easily listen for Hermes messages. Automatically unsubscribes
/// when the node is deleted.
/// <br></br>
/// <b> Usage: </b>
///<br></br>
/// var subNode = TreeUtils.CreateParentedChild&lt;HermesSubscriber&gt;(this);
///<br></br>
/// Hermes.Subscribe&lt;M_MeleeWeaponImpact&gt;(subNode, this.someDelegate);
/// </summary>
[GlobalClass]
public partial class HermesSubscriberNode : Node, IHermesSubscriber
{
    public List<TinyMessageSubscriptionToken> HermesTokens { get; } = new();

    public override void _ExitTree()
    {
        if (!IsQueuedForDeletion())
        {
            // Don't fire if we're just switching scenes!
            return;
        }

        foreach (TinyMessageSubscriptionToken token in HermesTokens)
            token.Dispose();
    }
}

