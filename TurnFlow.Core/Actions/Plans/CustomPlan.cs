

using System.Collections.Generic;
using TurnFlow.Core.Effects.Plans;

namespace TurnFlow.Core.Actions.Plans;

public struct CustomPlan
{
    public bool isDamage;
    public DamageEffectPlan? damageEffectPlan; // only used if isDamage is true

    public bool isBuff;
    public BuffDef? buffDef; // only used if isBuff is true

    public bool isDebuff;
    public DebuffDef? debuffDef; // only used if isDebuff is true
}

public enum ChangeDirectionType
{
    Damage,
    Heal,
}

public struct BuffDef
{
    public string buffStat;
    public int buffAmount;
    public BuffDurationScope buffDurationScope;
}

public struct DebuffDef
{
    public string debuffStat;
    public int debuffAmount;
    public BuffDurationScope debuffDurationScope;
}

public enum BuffDurationScope
{
    Battle,
}