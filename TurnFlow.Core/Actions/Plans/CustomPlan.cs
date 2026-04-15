

using System.Collections.Generic;

namespace TurnFlow.Core.Actions.Plans;

public struct CustomPlan
{
    public bool isBarChange;
    public BarChangeDef? barChangeDef; // only used if isBarChange is true

    public bool isBuff;
    public BuffDef? buffDef; // only used if isBuff is true

    public bool isDebuff;
    public DebuffDef? debuffDef; // only used if isDebuff is true
}

public struct BarChangeDef
{
    public ChangeDirectionType changeDirection;
    public int changeAmount;
    public string damageChangeTypeName;
    public string damageElementTypeName;
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