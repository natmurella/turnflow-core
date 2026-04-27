


using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TurnFlow.Core.Actions;
using TurnFlow.Core.Characters;
using TurnFlow.Core.Effects.Plans;
using TurnFlow.Core.Infos;
using TurnFlow.Core.Managers.Engines;
using TurnFlow.Core.Managers.Handles;
using TurnFlow.Core.Mechanics;
using TurnFlow.Core.Triggers;

namespace TurnFlow.Core.Effects.BasicEffects;

public class DamageEffect : IEffect
{
    private ICharacter source;
    private ICharacter target;
    private IAction? sourceAction;
    private ITrigger? sourceTrigger;
    private readonly DamageEffectPlan damageEffectPlan;

    public DamageEffect(
        IAction action,
        ICharacter source, 
        ICharacter target,
        DamageEffectPlan damageEffectPlan
    )
    {
        this.source = source;
        this.target = target;
        this.sourceAction = action;
        this.sourceTrigger = null;
        this.damageEffectPlan = damageEffectPlan;
    }

    public DamageEffect(
        ITrigger trigger,
        ICharacter source, 
        ICharacter target,
        DamageEffectPlan damageEffectPlan
    )
    {
        this.source = source;
        this.target = target;
        this.sourceAction = null;
        this.sourceTrigger = trigger;
        this.damageEffectPlan = damageEffectPlan;
    }

    public void ExecuteEffect(IExecuteEffectHandle engine)
    {
        IInfo info;

        if (this.sourceAction is IAction a)
        {
            info = new Info(a, source, target);

        }
        else if (this.sourceTrigger is ITrigger t)
        {
            info = new Info(t, source, target);
        }
        else
        {
            throw new Exception("Effect must have either a source action or trigger.");
        }

        // set damage direction type
        info.SetDamageDirectionType(this.damageEffectPlan.damageDirectionType);

        // trigger damage change calc
        engine.Trigger(
            "on_damage_change_resolve_open",
            info
        );
        String damageChangeType = ResolveDamageChangeType();
        engine.Trigger(
            "on_damage_change_resolve_close",
            info
        );
        info.SetDamageChangeType(damageChangeType);

        // trigger damage element calc
        engine.Trigger(
            "on_damage_element_resolve_open",
            info
        );
        String damageElementType = ResolveDamageElementType();
        engine.Trigger(
            "on_damage_element_resolve_close",
            info
        );
        info.SetDamageElementType(damageElementType);

        // calculate damage amount
        engine.Trigger(
            "on_damage_amount_calc_open",
            info
        );
        int damageAmount = CalculateDamageAmount();
        engine.Trigger(
            "on_damage_amount_calc_close",
            info
        );
        info.SetDamageAmount(damageAmount);

        // execute damage
        engine.Trigger(
            "on_damage_execute_open",
            info
        );
        List<TriggerParams> tp = Execute(info);
        engine.Trigger(
            "on_damage_execute_close",
            info
        );

        // trigger all resulting triggers
        engine.TriggerAll(tp);
    }

    private String ResolveDamageChangeType()
    {
        return BasicCollectionMechanics.ResolveDamageChangeType(this.damageEffectPlan.damageChangeTypeName, this.source);
    }

    private String ResolveDamageElementType()
    {
        return BasicCollectionMechanics.ResolveDamageElementType(this.damageEffectPlan.damageElementName, this.source);
    }

    private int CalculateDamageAmount()
    {
        return BasicCollectionMechanics.CalculateDamageAmount(
            this.source,
            this.target,
            this.damageEffectPlan
        );
    }

    private List<TriggerParams> Execute(IInfo info)
    {
        return BasicInteractionMechanics.DealDamageOrHeal(info);
    }
}