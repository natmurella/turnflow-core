using System.Collections.Generic;

namespace TurnFlow.Core.Components.Plans.StatPlans;

public struct DamageChangePlan
{
    public List<DamageChangeDef> damageChanges;
}

public struct DamageChangeDef
{
    public string damageChangeTypeName;
    public List<DamageChangeBarDef> barOrder; 
}

public struct DamageChangeBarDef
{
    public string barName;
    public bool isIgnored;
}