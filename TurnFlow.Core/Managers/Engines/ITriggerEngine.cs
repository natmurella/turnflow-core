

using System;
using TurnFlow.Core.Actions;
using TurnFlow.Core.Characters;
using TurnFlow.Core.Effects;
using TurnFlow.Core.Infos;
using TurnFlow.Core.Triggers;

namespace TurnFlow.Core.Managers.Engines;

public interface ITriggerEngine
{
    public void RegisterTrigger(
        ITrigger trigger
    );

    public void RegisterEffect(
        IEffect effect,
        IAction sourceAction
    );

    public void Trigger(
        String triggerType,
        IInfo info
    );
}