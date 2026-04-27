


using System.Collections.Generic;
using TurnFlow.Core.Characters;
using TurnFlow.Core.Components.Managers;
using TurnFlow.Core.Components.Plans.StatPlans;
using TurnFlow.Core.Infos;

namespace TurnFlow.Core.Triggers.SystemTriggers;

public class SystemDamageChangeTrigger : DamageTrigger
{
    protected DamageChangePlan damageChangePlan;
    protected Dictionary<string, List<string>> compiledPlan;

    public SystemDamageChangeTrigger(DamageChangePlan damageChangePlan) : base(1)
    {
        this.damageChangePlan = damageChangePlan;
        this.triggerType.Add("open", "on_damage_execute_open");
        this.triggerType.Add("close", "on_damage_execute_close");
        this.compiledPlan = CompilePlan(damageChangePlan);
    }

    private Dictionary<string, List<string>> CompilePlan(DamageChangePlan damageChangePlan)
    {
        Dictionary<string, List<string>> compiledPlan = new Dictionary<string, List<string>>();

        foreach (DamageChangeDef changeName in damageChangePlan.damageChanges)
        {
            List<string> barNames = new List<string>();
            foreach (DamageChangeBarDef barDef in changeName.barOrder)
            {
                barNames.Add(barDef.barName);
            }
            compiledPlan.Add(changeName.damageChangeTypeName, barNames);
        }

        return compiledPlan;
    }

    protected override void TriggerFire(string triggerType, IDamageInfo info)
    {
        ICharacter target = info.ToCharacter();
        string damageChangeType = info.GetDamageChangeType();
        
        if (triggerType == this.triggerType.GetValue("open"))
        {
            AddPlan(target, damageChangeType);
        }
        else if (triggerType == this.triggerType.GetValue("close"))
        {
            RemovePlan(target, damageChangeType);
        }
    }

    private void AddPlan(ICharacter target, string damageChangeType)
    {
        IComponentManager cm = target.GetComponentManager();

        foreach (string barName in compiledPlan[damageChangeType])
        {
            string on_key = $"damage_hit_bar_{damageChangeType}_include";

            cm.AddToBubble(on_key, 1, this, 0);
        }
    }

    private void RemovePlan(ICharacter target, string damageChangeType)
    {
        IComponentManager cm = target.GetComponentManager();

        foreach (string barName in compiledPlan[damageChangeType])
        {
            string on_key = $"damage_hit_bar_{damageChangeType}_include";

            cm.RemoveFromBubble(on_key, 1, this, 0);
        }
    }
}