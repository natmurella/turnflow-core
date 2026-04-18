


using System;
using System.Runtime.CompilerServices;
using TurnFlow.Core.Actions;
using TurnFlow.Core.Characters;
using TurnFlow.Core.Infos;
using TurnFlow.Core.Managers.Handles;
using TurnFlow.Core.Mechanics;
using TurnFlow.Core.Triggers;

namespace TurnFlow.Core.Effects;

public abstract class DamageEffect : IEffect
{
    private ICharacter source;
    private ICharacter target;
    private IAction? sourceAction;
    private ITrigger? sourceTrigger;
    private String damageChange;
    private String damageElement;

    public DamageEffect(
        IAction action,
        ICharacter source, 
        ICharacter target,
        String damageChange,
        String damageElement
    )
    {
        this.source = source;
        this.target = target;
        this.sourceAction = action;
        this.sourceTrigger = null;
        this.damageChange = damageChange;
        this.damageElement = damageElement;
    }

    public DamageEffect(
        ITrigger trigger,
        ICharacter source, 
        ICharacter target,
        String damageChange,
        String damageElement
    )
    {
        this.source = source;
        this.target = target;
        this.sourceAction = null;
        this.sourceTrigger = trigger;
        this.damageChange = damageChange;
        this.damageElement = damageElement;
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

        // trigger damage change calc
        engine.Trigger(
            "on_damage_change_calc_open",
            info
        );
        String damageChangeType = CalculateDamageChangeType(this.damageChange, source);
        engine.Trigger(
            "on_damage_change_calc_close",
            info
        );
        info.SetDamageChangeType(damageChangeType);

        // trigger damage element calc
        engine.Trigger(
            "on_damage_element_calc_open",
            info
        );
        String damageElementType = CalculateDamageElementType(this.damageElement, source);
        engine.Trigger(
            "on_damage_element_calc_close",
            info
        );
        info.SetDamageElementType(damageElementType);

        // calculate damage amount
        engine.Trigger(
            "on_damage_amount_calc_open",
            info
        );
        int damageAmount = CalculateDamageAmount(info);
        engine.Trigger(
            "on_damage_amount_calc_close",
            info
        );
        info.SetDamageAmount(damageAmount);

        // execute damage
        Execute(engine, info);
    }

    private String CalculateDamageChangeType(String baseDamageChange, ICharacter fromCharacter)
    {
        return BasicCollectionMechanics.CalculateDamageChangeType(baseDamageChange, fromCharacter);
    }

    private String CalculateDamageElementType(String baseDamageElement, ICharacter fromCharacter)
    {
        return BasicCollectionMechanics.CalculateDamageElementType(baseDamageElement, fromCharacter);
    }

    protected abstract int CalculateDamageAmount(IInfo info);

    protected abstract void Execute(IEffectHandle engine, IInfo info);
}