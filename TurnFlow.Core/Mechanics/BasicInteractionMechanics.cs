


using System.Collections.Generic;
using System.Linq;
using TurnFlow.Core.Characters;
using TurnFlow.Core.Components.Managers;
using TurnFlow.Core.Effects.Plans;
using TurnFlow.Core.Infos;
using TurnFlow.Core.Managers.Engines;

namespace TurnFlow.Core.Mechanics;

public static class BasicInteractionMechanics
{
    
    public static List<TriggerParams> DealDamageOrHeal(
        IInfo info
    )
    {

        List<TriggerParams> triggers = new List<TriggerParams>();

        IComponentManager source = info.FromCharacter().GetComponentManager();
        IComponentManager target = info.ToCharacter().GetComponentManager();

        // get amount
        int amount = info.GetDamageAmount();
        DamageDirectionType damageDirectionType = info.GetDamageDirectionType();

        List<string> bars = source.GetEnum("bars");

        IEnumerable<string> barsOrdered = damageDirectionType == DamageDirectionType.Damage 
            ? bars 
            : bars.AsEnumerable().Reverse();

        foreach (string bar in barsOrdered)
        {
            // skip non included bars
            int hitBar = source.Read("damage_hit_bar_" + bar + "_include");
            if (hitBar < 1)
            {
                continue;
            }

            // apply damage or heal
            int barCur = target.Read($"bar_cur_{bar}");
            int barMax = target.Read($"bar_max_{bar}");
            int damage;
            int overDamage;
            string barDirectionPrefix;
            bool needsCharacterSwap;
            if (damageDirectionType == DamageDirectionType.Damage)
            {
                damage = System.Math.Min(amount, barCur);
                overDamage = System.Math.Max(0, amount - barCur);
                target.Remove($"bar_cur_{bar}", damage);
                barDirectionPrefix = "damage";
                needsCharacterSwap = false;
            }
            else
            {
                damage = System.Math.Min(amount, barMax - barCur);
                overDamage = System.Math.Max(0, amount - (barMax - barCur));
                target.Add($"bar_cur_{bar}", damage);
                barDirectionPrefix = "heal";
                needsCharacterSwap = true;
            }

            // create trigger params for this bar
            // damage trigger params
            if (damage > 0)
            {
                // on damage/heal dealt trigger
                Info dealtInfo = new Info(info, needsCharacterSwap);
                dealtInfo.SetDamageAmount(damage);
                dealtInfo.SetDamageBar(bar);
                string onDamageDealtTriggerType = $"on_{barDirectionPrefix}_dealt";
                TriggerParams onDealtTriggerParams = new TriggerParams(onDamageDealtTriggerType, dealtInfo);
                triggers.Add(onDealtTriggerParams);
                // on damage/heal received trigger
                Info takenInfo = new Info(dealtInfo, true);
                string onDamageTakenTriggerType = $"on_{barDirectionPrefix}_taken";
                TriggerParams onTakenTriggerParams = new TriggerParams(onDamageTakenTriggerType, takenInfo);
                triggers.Add(onTakenTriggerParams);
            }
            // overdamage trigger params
            if (overDamage > 0)
            {
                // on overdamage/heal dealt trigger
                Info overDealtInfo = new Info(info, needsCharacterSwap);
                overDealtInfo.SetDamageAmount(overDamage);
                overDealtInfo.SetDamageBar(bar);
                string onOverDealtTriggerType = $"on_over{barDirectionPrefix}_dealt";
                TriggerParams onOverDealtTriggerParams = new TriggerParams(onOverDealtTriggerType, overDealtInfo);
                triggers.Add(onOverDealtTriggerParams);
                // on overdamage/heal received trigger
                Info overTakenInfo = new Info(overDealtInfo, true);
                string onOverTakenTriggerType = $"on_over{barDirectionPrefix}_taken";
                TriggerParams onOverTakenTriggerParams = new TriggerParams(onOverTakenTriggerType, overTakenInfo);
                triggers.Add(onOverTakenTriggerParams);
            }

            amount -= damage;
            if (amount <= 0)
            {
                break;
            }
        }

        return triggers;
    }

}