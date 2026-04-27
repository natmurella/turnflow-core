
using System;
using System.Collections.Generic;
using TurnFlow.Core.Components.Managers;

namespace TurnFlow.Core.Components.Plans.Processors;

public class BarProcessor : IProcessor
{
    private Dictionary<string, string> componentsToUse;

    public BarProcessor(
        Dictionary<string, string> componentsToUse
    ) : base()
    {
        this.componentsToUse = componentsToUse;
    }

    public int Recalculate(IComponentManager componentManager)
    {
        int base_add_mult = componentManager.Read<int>(componentsToUse["add_mult"]);

        if (base_add_mult == 0)
        {
            return 0;
        }

        int base_add_flat = componentManager.Read<int>(componentsToUse["add_flat"]);
        int base_min_flat = componentManager.Read<int>(componentsToUse["min_flat"]);
        int base_min_mult = componentManager.Read<int>(componentsToUse["min_mult"]);

        // add the multiplier by state to add flat
        string prefix = componentsToUse["prefix"];
        foreach (string stat in componentManager.GetEnum("stats"))
        {
            int add_mult = componentManager.Read<int>($"{prefix}_{stat}_add_mult");
            if (add_mult == 0)
            {
                continue;
            }

            int stat_value = componentManager.Read<int>($"stat_{stat}");
            if (stat_value == 0)
            {
                continue;
            }

            float mult_factor = add_mult / 100f;
            base_add_flat += (int)Math.Round(stat_value * mult_factor, MidpointRounding.AwayFromZero);
        }

        if (base_add_flat == 0)
        {
            return 0;
        }

        int base_value = Math.Max(base_add_flat - base_min_flat, 0);

        if (base_value == 0)
        {
            return 0;
        }

        int base_mult = Math.Max(base_add_mult - base_min_mult, 0);

        if (base_mult == 0)
        {
            return 0;
        }

        float final_mult_factor = base_mult / 100f;

        int final_value = (int)Math.Round(base_value * final_mult_factor, MidpointRounding.AwayFromZero);

        return final_value;
    }
}