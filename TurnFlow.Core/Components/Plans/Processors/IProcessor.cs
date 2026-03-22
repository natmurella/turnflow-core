


using System.Collections.Generic;
using TurnFlow.Core.Components.Managers;

namespace TurnFlow.Core.Components.Plans.Processors;

public interface IProcessor
{
    public int Recalculate(IComponentManager componentManager);
}