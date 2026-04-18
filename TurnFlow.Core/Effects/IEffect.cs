


using TurnFlow.Core.Infos;
using TurnFlow.Core.Managers.Handles;

namespace TurnFlow.Core.Effects;

public interface IEffect
{
    public void ExecuteEffect(IExecuteEffectHandle engine);
}