


using System.Collections.Generic;
using TurnFlow.Core.Characters;
using TurnFlow.Core.Infos;
using TurnFlow.Core.Utility;

namespace TurnFlow.Core.Triggers;

public abstract class DamageTrigger : Trigger
{

    public DamageTrigger(int duration = 1) : base(duration)
    {
        
    }

    public override void Fire(string triggerType, IInfo info)
    {
        TriggerFire(triggerType, info);
    }

    protected abstract void TriggerFire(string triggerType, IDamageInfo info);
}