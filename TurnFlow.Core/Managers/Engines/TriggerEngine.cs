



using System.Collections.Generic;
using TurnFlow.Core.Actions;
using TurnFlow.Core.Characters;
using TurnFlow.Core.Effects;
using TurnFlow.Core.Infos;
using TurnFlow.Core.Managers.Handles;
using TurnFlow.Core.Triggers;

namespace TurnFlow.Core.Managers.Engines;

public class TriggerEngine : ITriggerEngine, IEffectHandle, IActionHandle, IExecuteEffectHandle
{
    private Dictionary<string, List<ITrigger>> triggerList;
    private Stack<IEffect> effectStack;

    public TriggerEngine()
    {
        triggerList = new Dictionary<string, List<ITrigger>>();
        this.effectStack = new Stack<IEffect>();
    }

    public void RegisterTrigger(
        ITrigger trigger
    ) 
    {
        string t_type = trigger.GetTriggerType();
        if (!triggerList.ContainsKey(t_type))
        {
            triggerList[t_type] = new List<ITrigger>();
        }

        triggerList[t_type].Add(trigger);
    }

    public void RegisterEffect(
        IEffect effect,
        IAction sourceAction
    )
    {
        effectStack.Push(effect);
    }

    public void Trigger(
        string triggerType,
        IInfo info
    )
    {
        if (!triggerList.ContainsKey(triggerType))
        {
            return;
        }

        var triggers = triggerList[triggerType];
        List<ITrigger> triggersToKeep = new List<ITrigger>();

        foreach (ITrigger t in triggers)
        {
            t.Fire(
                triggerType,
                info
            );

            if (!t.IsDurationZero())
            {
                triggersToKeep.Add(t);
            }
        }

        if (triggersToKeep.Count == 0)
        {
            triggerList.Remove(triggerType);
        }
        else
        {
            triggerList[triggerType] = triggersToKeep;
        }
    }

    public void TriggerAll(List<TriggerParams> triggerParamsList)
    {
        foreach (TriggerParams triggerParams in triggerParamsList)
        {
            Trigger(
                triggerParams.triggerType,
                triggerParams.info
            );
        }
    }
}