


using TurnFlow.Core.Characters;
using TurnFlow.Core.Infos;

namespace TurnFlow.Core.Managers.Engines;

public struct TriggerParams
{
    public string triggerType;
    public IInfo info;

    public TriggerParams(string triggerType, IInfo info)
    {
        this.triggerType = triggerType;
        this.info = info;
    }
}