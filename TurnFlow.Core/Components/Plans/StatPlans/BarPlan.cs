
using System.Collections.Generic;

namespace TurnFlow.Core.Components.Plans.StatPlans;

public struct BarPlan
{
    public List<BarDef> bars;
}

public struct BarDef
{
    public string barName;
    public BarResetType resetType;
    public bool allowOverMax;
    public BarMaxCalcType maxCalcType;
    public int maxValue;
    public int baseBarValue;
    public List<BarFormulaDef> formulaDefs;

}

public struct BarFormulaDef
{
    public string statName;
    public float multiplier;
}

public enum BarResetType
{
    toMinimum,
    toMaximum,
    noChange,
}

public enum BarMaxCalcType
{
    target,
    formula,
}

