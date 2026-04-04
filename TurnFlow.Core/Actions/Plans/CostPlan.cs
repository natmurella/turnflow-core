

using System.Collections.Generic;

namespace TurnFlow.Core.Actions.Plans;

public struct CostPlan
{
    public List<BarCostDef> barCosts;
    public List<ResourceCostDef> resourceCosts;
}

public struct BarCostDef
{
    public string barName;
    public int costValue;
}

public struct ResourceCostDef
{
    public string resourceName;
    public int costValue;
}