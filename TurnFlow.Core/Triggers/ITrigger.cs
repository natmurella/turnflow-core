

using TurnFlow.Core.Characters;
using TurnFlow.Core.Infos;

namespace TurnFlow.Core.Triggers;

public interface ITrigger
{
    public bool IsDurationZero();
    public string GetTriggerType();
    public void Fire(string triggerType, ICharacter target, IInfo info);
}