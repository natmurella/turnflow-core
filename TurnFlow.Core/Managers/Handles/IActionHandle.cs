

using TurnFlow.Core.Actions;
using TurnFlow.Core.Effects;
using TurnFlow.Core.Triggers;

namespace TurnFlow.Core.Managers.Handles;

public interface IActionHandle
{
    public void RegisterTrigger(ITrigger trigger);
    public void RegisterEffect(IEffect effect, IAction sourceAction);
}