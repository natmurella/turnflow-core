

using System;
using System.Collections.Generic;
using TurnFlow.Core.Actions;
using TurnFlow.Core.Characters;
using TurnFlow.Core.Effects;
using TurnFlow.Core.Effects.Plans;
using TurnFlow.Core.Triggers;

namespace TurnFlow.Core.Infos;

public  class Info : IInfo
{

    protected ITrigger? fromTrigger;
    protected IEffect? fromEffect;
    protected IAction? fromAction;
    protected ICharacter fromCharacter;
    protected ICharacter toCharacter;
    
    // damage info
    protected DamageDirectionType? damageDirectionType;
    protected String? damageChangeType;
    protected String? damageElementType;
    protected int? damageAmount;
    protected string? damageBar;


    public Info(ITrigger? fromTrigger, IEffect? fromEffect, IAction? fromAction, ICharacter fromCharacter, ICharacter toCharacter)
    {
        this.fromTrigger = fromTrigger;
        this.fromEffect = fromEffect;
        this.fromAction = fromAction;
        this.fromCharacter = fromCharacter;
        this.toCharacter = toCharacter;
    }

    public Info(IAction action, ICharacter fromCharacter, ICharacter toCharacter)
    {
        this.fromTrigger = null;
        this.fromEffect = null;
        this.fromAction = action;
        this.fromCharacter = fromCharacter;
        this.toCharacter = toCharacter;
    }

    public Info(ITrigger trigger, ICharacter fromCharacter, ICharacter toCharacter)
    {
        this.fromTrigger = trigger;
        this.fromEffect = null;
        this.fromAction = null;
        this.fromCharacter = fromCharacter;
        this.toCharacter = toCharacter;
    }

    public Info(IInfo info, bool characterSwap=false)
    {
        // general
        this.fromTrigger = info.FromTrigger();
        this.fromEffect = info.FromEffect();
        this.fromAction = info.FromAction();
        if (characterSwap)
        {
            this.fromCharacter = info.ToCharacter();
            this.toCharacter = info.FromCharacter();
        }
        else
        {
            this.fromCharacter = info.FromCharacter();
            this.toCharacter = info.ToCharacter();
        }
        // damage info
        this.damageDirectionType = info.GetDamageDirectionType();
        this.damageChangeType = info.GetDamageChangeType();
        this.damageElementType = info.GetDamageElementType();
        this.damageAmount = info.GetDamageAmount();
        this.damageBar = info.GetDamageBar();
    }

    // general

    public ITrigger? FromTrigger()
    {
        return fromTrigger;
    }

    public IEffect? FromEffect()
    {
        return fromEffect;
    }

    public IAction? FromAction()
    {
        return fromAction;
    }

    public ICharacter FromCharacter()
    {
        return fromCharacter;
    }

    public ICharacter ToCharacter()
    {
        return toCharacter;
    }

    // base

    public void SetTrigger(ITrigger trigger)
    {
        this.fromTrigger = trigger;
    }

    public void SetEffect(IEffect effect)
    {
        this.fromEffect = effect;
    }

    public void SetAction(IAction action)
    {
        this.fromAction = action;
    }

    public void SetFromCharacter(ICharacter fromCharacter)
    {
        this.fromCharacter = fromCharacter;
    }

    public void SetToCharacter(ICharacter toCharacter)
    {
        this.toCharacter = toCharacter;
    }

    // damage info

    public void SetDamageDirectionType(DamageDirectionType damageDirectionType)
    {
        this.damageDirectionType = damageDirectionType;
    }

    public void SetDamageChangeType(String damageChangeType)
    {
        this.damageChangeType = damageChangeType;
    }

    public void SetDamageElementType(String damageElementType)
    {
        this.damageElementType = damageElementType;
    }

    public void SetDamageAmount(int damageAmount)
    {
        this.damageAmount = damageAmount;
    }

    public void SetDamageBar(string damageBar)
    {
        this.damageBar = damageBar;
    }

    public DamageDirectionType GetDamageDirectionType()
    {
        if (damageDirectionType == null)
        {
            throw new Exception("Damage direction type is not set");
        }
        return (DamageDirectionType)damageDirectionType;
    }

    public String GetDamageChangeType()
    {
        if (damageChangeType == null)
        {
            throw new Exception("Damage change type is not set");
        }
        return damageChangeType;
    }

    public String GetDamageElementType()
    {
        if (damageElementType == null)
        {
            throw new Exception("Damage element type is not set");
        }
        return damageElementType;
    }

    public int GetDamageAmount()
    {
        if (damageAmount == null)
        {
            throw new Exception("Damage amount is not set");
        }
        return (int)damageAmount;
    }

    public string GetDamageBar()
    {
        if (damageBar == null)
        {
            throw new Exception("Damage bar is not set");
        }
        return damageBar;
    }
    
}