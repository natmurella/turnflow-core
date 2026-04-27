using System.Collections.Generic;
using TurnFlow.Core.Characters;
using TurnFlow.Core.Infos;
using TurnFlow.Core.Managers.Engines;

namespace TurnFlow.Core.Managers.Handles;

public interface IExecuteEffectHandle : IEffectHandle
{
    public void Trigger(
        string triggerType,
        IInfo info
    );

    public void TriggerAll(List<TriggerParams> triggerParams);
}