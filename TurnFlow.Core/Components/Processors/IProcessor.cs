


using System.Collections.Generic;

namespace TurnFlow.Core.Components.Processors;

public interface IProcessor
{
    public int Recalculate();
    public void ApplyDependecyGroup(ComponentDependencyGroup group);
}