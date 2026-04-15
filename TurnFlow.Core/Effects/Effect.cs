


using TurnFlow.Core.Characters;

namespace TurnFlow.Core.Effects;

public abstract class Effect : IEffect
{
    protected ICharacter source;
    protected ICharacter target;

    public Effect(ICharacter source, ICharacter target)
    {
        this.source = source;
        this.target = target;
    }

    public abstract void Execute();
}