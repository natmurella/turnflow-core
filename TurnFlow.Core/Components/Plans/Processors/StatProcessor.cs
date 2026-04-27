
using System;
using System.Collections.Generic;
using TurnFlow.Core.Components.Managers;

namespace TurnFlow.Core.Components.Plans.Processors;

public class StatProcessor : IProcessor
{
    private Dictionary<string, string> componentsToUse;

    public StatProcessor(
        Dictionary<string, string> componentsToUse
    ) : base()
    {
        this.componentsToUse = componentsToUse;
    }

    public int Recalculate(IComponentManager componentManager)
    {
        int flat = Math.Max(
            componentManager.Read<int>(componentsToUse["add_flat"]) - componentManager.Read<int>(componentsToUse["min_flat"]),
            0
        );

        if (flat == 0)
        {
            return 0;
        }

        int mult = Math.Max(
            componentManager.Read<int>(componentsToUse["add_mult"]) - componentManager.Read<int>(componentsToUse["min_mult"]),
            0
        );

        if (mult == 0)
        {
            return 0;
        }

        float mult_factor = mult / 100f;

        int final_value = (int)Math.Round(flat * mult_factor, MidpointRounding.AwayFromZero);

        return final_value;
    }
}