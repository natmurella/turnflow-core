


using TurnFlow.Core.Characters;
using TurnFlow.Core.Infos;

namespace TurnFlow.Core.Managers.Engines;

public struct TriggerParams
{
    public string triggerType;
    public ICharacter target;
    public IInfo info;
}