


using System.Collections.Generic;

namespace TurnFlow.Core.Effects.Plans;

public struct DamageEffectPlan
{
    public string damageChangeTypeName;
    public string damageElementName;
    
    // amount variables
    public int baseAmount;
    public Dictionary<string, int> statSourceScalingDict;
}