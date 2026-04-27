


using System;
using System.Collections.Generic;
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

        return damageChange;
    }

    public static String ResolveDamageElementType(String baseDamageElement, ICharacter fromCharacter)
    {
        IComponentManager cm = fromCharacter.GetComponentManager();

        string out_element = cm.Read<string>("damage_element");

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
}