


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

        List<string> damageChangeTypes = cm.GetEnum("damage_changes");
        string outputDamageChangeType = baseDamageChange;

        foreach (string damageChangeType in damageChangeTypes)
        {
            string on_key = $"damage_change_{damageChangeType}_on";
            string off_key = $"damage_change_{damageChangeType}_off";

            if (cm.Read(on_key) > 0 && cm.Read(off_key) == 0)
            {
                outputDamageChangeType = damageChangeType;
            }
        }

        return outputDamageChangeType;
    }

    public static String ResolveDamageElementType(String baseDamageElement, ICharacter fromCharacter)
    {
        IComponentManager cm = fromCharacter.GetComponentManager();

        List<string> damageElementTypes = cm.GetEnum("damage_elements");
        string outputDamageElementType = baseDamageElement;

        foreach (string damageElementType in damageElementTypes)
        {
            string on_key = $"damage_element_{damageElementType}_on";
            string off_key = $"damage_element_{damageElementType}_off";

            if (cm.Read(on_key) > 0 && cm.Read(off_key) == 0)
            {
                outputDamageElementType = damageElementType;
            }
        }

        return outputDamageElementType;
    }

    public static int CalculateDamageAmount(ICharacter source, ICharacter target, DamageEffectPlan damageEffectPlan)
    {
        IComponentManager cm = source.GetComponentManager();
        IComponentManager target_cm = target.GetComponentManager();

        // base damage
        int baseDamage = damageEffectPlan.baseAmount;
        baseDamage += cm.Read("damage_add_flat");
        baseDamage -= cm.Read("damage_min_flat");

        // stat scaling
        List<string> statEnum = cm.GetEnum("stats");
        foreach (string statName in statEnum)
        {
            int stat_add_mult = damageEffectPlan.statSourceScalingDict.GetValueOrDefault(statName, 0);
            stat_add_mult += cm.Read($"damage_stat_source_{statName}_add_mult");

            if (stat_add_mult == 0)
            {
                continue;
            }

            int statValue = cm.Read($"stat_{statName}");

            if (statValue == 0)
            {
                continue;
            }

            int scaled_stat_contribution = (int) (statValue * (stat_add_mult / 100.0));
            baseDamage += scaled_stat_contribution;
        }

        // base damage multipliers
        int baseMultDamage = cm.Read("damage_add_mult");
        baseMultDamage -= cm.Read("damage_min_mult");

        if (baseDamage <= 0 || baseMultDamage <= 0)
        {
            return 0;
        }

        int finalDamage = (int) (baseDamage * (baseMultDamage / 100.0));

        return finalDamage;
    }
}