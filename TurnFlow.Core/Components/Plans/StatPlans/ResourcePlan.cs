
using System.Collections.Generic;

namespace TurnFlow.Core.Components.Plans.StatPlans;

public struct ResourcePlan
{
    public List<ResourceDef> resources;
}

public struct ResourceDef
{
    public string resourceName;
}