


using System.Collections.Generic;
using TurnFlow.Core.Characters;
using TurnFlow.Core.Infos;
using TurnFlow.Core.Utility;

namespace TurnFlow.Core.Triggers;

public abstract class Trigger : ITrigger
{
    protected SymmetricDictionary<string, string> triggerType;
    protected int duration;

    public Trigger(int duration = 1)
    {
        this.triggerType = new SymmetricDictionary<string, string>();
        this.duration = duration;
    }

    public bool IsDurationZero()
    {
        return duration <= 0;
    }

    public HashSet<string> GetTriggerTypes()
    {
        return triggerType.GetValues();
    }

    public abstract void Fire(string triggerType, IInfo info);
}