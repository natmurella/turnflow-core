

using System.Collections.Generic;
using TurnFlow.Core.Components.Processors;

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

    public Component(IProcessor processor) : this()
    {
        this.processor = processor;
        groupUsedForCalculation = new ComponentDependencyGroup();
        processor.ApplyDependecyGroup(groupUsedForCalculation);
    }

    public int Read()
    {
        if (this.processor is IProcessor p && groupUsedForCalculation is ComponentDependencyGroup g)
        {
            if (g.HasChanged())
            {
                this.value = p.Recalculate();
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

    public void Remove(int value)
    {
        if (value == 0)
        {
            return;
        }

        this.value -= value;

        foreach (var group in groupsAffectedByChange)
        {
            group.MarkChanged();
        }
    }

    public void AddToGroup(ComponentDependencyGroup group)
    {
        if (!groupsAffectedByChange.Contains(group))
        {
            groupsAffectedByChange.Add(group);
        }
    }
}