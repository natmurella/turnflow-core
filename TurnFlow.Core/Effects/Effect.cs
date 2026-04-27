


using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TurnFlow.Core.Actions;
using TurnFlow.Core.Characters;
using TurnFlow.Core.Infos;
using TurnFlow.Core.Managers.Engines;
using TurnFlow.Core.Managers.Handles;
using TurnFlow.Core.Triggers;

namespace TurnFlow.Core.Effects;

public abstract class Effect : IEffect
{
    private ICharacter source;
    private ICharacter target;
    private IAction? sourceAction;
    private ITrigger? sourceTrigger;

    public Effect(
        IAction action,
        ICharacter source, 
        ICharacter target
    )
    {
        this.source = source;
        this.target = target;
        this.sourceAction = action;
        this.sourceTrigger = null;
    }

    public Effect(
        ITrigger trigger,
        ICharacter source, 
        ICharacter target
    )
    {
        this.source = source;
        this.target = target;
        this.sourceAction = null;
        this.sourceTrigger = trigger;
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

        List<TriggerParams> tp = Execute(engine, info);
        engine.TriggerAll(tp);

    }

    protected abstract List<TriggerParams> Execute(IEffectHandle engine, IInfo info);
}