


using TurnFlow.Core.Characters;
using TurnFlow.Core.Infos;

namespace TurnFlow.Core.Triggers;

public abstract class Trigger : ITrigger
{
    protected string triggerType;
    protected int duration;

    public Trigger(string triggerType, int duration = 1)
    {
        this.triggerType = triggerType;
        this.duration = duration;
    }

    public bool IsDurationZero()
    {
        return duration <= 0;
    }

    public string GetTriggerType()
    {
        return triggerType;
    }
    public virtual void Fire(string triggerType, ICharacter target, IInfo info)
    {
        
    }
}