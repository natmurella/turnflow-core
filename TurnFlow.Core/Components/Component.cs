

using System;
using System.Collections.Generic;
using TurnFlow.Core.Components.Managers;
using TurnFlow.Core.Components.Plans.Processors;

namespace TurnFlow.Core.Components;


public class Component : IComponent
{
    private int value;
    private List<ComponentDependencyGroup> groupsAffectedByChange;
    private IProcessor? processor;
    private ComponentDependencyGroup? groupUsedForCalculation;

    public Component()
    {
        value = 0;
        groupsAffectedByChange = new List<ComponentDependencyGroup>();
        processor = null;
        groupUsedForCalculation = null;
    }

    public Component(IProcessor processor, ComponentDependencyGroup g) : this()
    {
        this.processor = processor;
        this.groupUsedForCalculation = g;
        g.MarkChanged();
    }

    public int Read(IComponentManager componentManager)
    {
        if (this.processor is IProcessor p && groupUsedForCalculation is ComponentDependencyGroup g)
        {
            if (g.HasChanged())
            {
                this.value = p.Recalculate(componentManager);
                g.MarkCalculated();
            }
        }

        return this.value;
    }

    public void Add(int value)
    {
        if (value == 0)
        {
            return;
        }

        this.value += value;

        foreach (var group in groupsAffectedByChange)
        {
            group.MarkChanged();
        }
    }

    // returns whether the final values is 0 or not.
    public bool Remove(int value)
    {
        if (value == 0)
        {
            return false;
        }

        if (this.value < value)
        {
            throw new InvalidOperationException($"Component.Remove: Attempting to remove {value} from component with value {this.value}.");
        }

        this.value -= value;

        foreach (var group in groupsAffectedByChange)
        {
            group.MarkChanged();
        }

        return this.value == 0;
    }

    public void AddToGroup(ComponentDependencyGroup group)
    {
        if (!groupsAffectedByChange.Contains(group))
        {
            groupsAffectedByChange.Add(group);
        }
    }
}