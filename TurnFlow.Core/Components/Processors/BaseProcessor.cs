


using System.Collections.Generic;

namespace TurnFlow.Core.Components.Processors;


public abstract class BaseProcessor : IProcessor
{
    protected Dictionary<string, IComponent> dependentComponents;
    protected BaseProcessor(
        Dictionary<string, IComponent> components
    )
    {
        dependentComponents = components;
    }
    public abstract int Recalculate();
    public void ApplyDependecyGroup(ComponentDependencyGroup group)
    {
        foreach (var component in dependentComponents.Values)
        {
            component.AddToGroup(group);
        }
    }
}