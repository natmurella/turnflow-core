


using System;
using System.Collections.Generic;
using TurnFlow.Core.Actions.Plans;
using TurnFlow.Core.Characters;
using TurnFlow.Core.Components.Managers;
using TurnFlow.Core.Effects.Plans;
using TurnFlow.Core.Infos;
using TurnFlow.Core.Managers.Engines;

namespace TurnFlow.Core.Mechanics;

public static class BasicCollectionMechanics
{
    
    public static String ResolveDamageChangeType(String baseDamageChange, ICharacter fromCharacter)
    {
        IComponentManager cm = fromCharacter.GetComponentManager();

        string damageChange = cm.Read<string>("damage_change");

        if (damageChange == "empty")
        {
            return baseDamageChange;
        }

        return damageChange;
    }

    public static String ResolveDamageElementType(String baseDamageElement, ICharacter fromCharacter)
    {
        IComponentManager cm = fromCharacter.GetComponentManager();

        string out_element = cm.Read<string>("damage_element");

        if (out_element == "empty")
        {
            return baseDamageElement;
        }

        return out_element;
    }

    public static int CalculateDamageAmount(ICharacter source, ICharacter target, DamageEffectPlan damageEffectPlan)
    {
        IComponentManager cm = source.GetComponentManager();
        IComponentManager target_cm = target.GetComponentManager();

        // base damage
        int baseDamage = damageEffectPlan.baseAmount;
        baseDamage += cm.Read<int>("damage_add_flat");
        baseDamage -= cm.Read<int>("damage_min_flat");

        // stat scaling
        List<string> statEnum = cm.GetEnum("stats");
        foreach (string statName in statEnum)
        {
            int stat_add_mult = damageEffectPlan.statSourceScalingDict.GetValueOrDefault(statName, 0);
            stat_add_mult += cm.Read<int>($"damage_stat_source_{statName}_add_mult");

            if (stat_add_mult == 0)
            {
                continue;
            }

            int statValue = cm.Read<int>($"stat_{statName}");

            if (statValue == 0)
            {
                continue;
            }

            int scaled_stat_contribution = (int) (statValue * (stat_add_mult / 100.0));
            baseDamage += scaled_stat_contribution;
        }

        // base damage multipliers
        int baseMultDamage = cm.Read<int>("damage_add_mult");
        baseMultDamage -= cm.Read<int>("damage_min_mult");

        if (baseDamage <= 0 || baseMultDamage <= 0)
        {
            return 0;
        }

        int finalDamage = (int) (baseDamage * (baseMultDamage / 100.0));

        return finalDamage;
    }

    public static bool ActionCanPayCost(ICharacter source, CostPlan costPlan)
    {
        IComponentManager cm = source.GetComponentManager();

        // check bar costs
        foreach (BarCostDef barCost in costPlan.barCosts)
        {
            int barValue = cm.Read<int>($"bar_cur_{barCost.barName}");
            if (barValue < barCost.costValue)
            {
                return false;
            }
        }

        // check resource costs
        foreach (ResourceCostDef resourceCost in costPlan.resourceCosts)
        {
            int resourceValue = cm.Read<int>($"resource_{resourceCost.resourceName}");
            if (resourceValue < resourceCost.costValue)
            {
                return false;
            }
        }

        return true;
    }

    public static CostPlan CalculateActionCostPlan(ICharacter source, CostPlan baseCost)
    {
        IComponentManager cm = source.GetComponentManager();

        Dictionary<string, int> barDict = new Dictionary<string, int>();
        Dictionary<string, int> resourceDict = new Dictionary<string, int>();

        foreach (BarCostDef barCost in baseCost.barCosts)
        {
            string barName = barCost.barName;
            int costValue = barCost.costValue;
            
            if (!barDict.TryAdd(barName, costValue))
            {
                barDict[barName] += costValue;
            }
        }

        foreach (ResourceCostDef resourceCost in baseCost.resourceCosts)
        {
            string resourceName = resourceCost.resourceName;
            int costValue = resourceCost.costValue;
            
            if (!resourceDict.TryAdd(resourceName, costValue))
            {
                resourceDict[resourceName] += costValue;
            }
        }

        CostPlan outCostPlan = new CostPlan()
        {
            barCosts = new List<BarCostDef>(),
            resourceCosts = new List<ResourceCostDef>(),
        };

        List<string> barEnum = cm.GetEnum("bars");
        foreach (string barName in barEnum)
        {
            int addFlat = cm.Read<int>($"bar_cost_{barName}_add_flat");
            int minFlat = cm.Read<int>($"bar_cost_{barName}_min_flat");
            int addMult = cm.Read<int>($"bar_cost_{barName}_add_mult");
            int minMult = cm.Read<int>($"bar_cost_{barName}_min_mult");

            int baseCostValue = barDict.GetValueOrDefault(barName, 0);
            baseCostValue += addFlat - minFlat;

            if (baseCostValue <= 0)
            {
                continue;
            }

            int multiplier = addMult - minMult;

            if (multiplier <= 0)
            {
                continue;
            }

            int finalCostValue = (int) (baseCostValue * (multiplier / 100.0));
            outCostPlan.barCosts.Add(
                new BarCostDef() { 
                    barName = barName, 
                    costValue = finalCostValue 
                }
            );
        }

        List<string> resourceEnum = cm.GetEnum("resources");
        foreach (string resourceName in resourceEnum)
        {
            int addFlat = cm.Read<int>($"resource_cost_{resourceName}_add_flat");
            int minFlat = cm.Read<int>($"resource_cost_{resourceName}_min_flat");
            int addMult = cm.Read<int>($"resource_cost_{resourceName}_add_mult");
            int minMult = cm.Read<int>($"resource_cost_{resourceName}_min_mult");

            int baseCostValue = resourceDict.GetValueOrDefault(resourceName, 0);
            baseCostValue += addFlat - minFlat;

            if (baseCostValue <= 0)
            {
                continue;
            }

            int multiplier = addMult - minMult;

            if (multiplier <= 0)
            {
                continue;
            }

            int finalCostValue = (int) (baseCostValue * (multiplier / 100.0));
            outCostPlan.resourceCosts.Add(
                new ResourceCostDef() { 
                    resourceName = resourceName, 
                    costValue = finalCostValue 
                }
            );
        }

        return outCostPlan;
    }
}