using System.Collections.Generic;

namespace TurnFlow.Core.Components.Plans.StatPlans;

public struct DamageElementPlan
{
    public List<DamageElementDef> damageElements;
}

public struct DamageElementDef
{
    public string damageElementName; 
}