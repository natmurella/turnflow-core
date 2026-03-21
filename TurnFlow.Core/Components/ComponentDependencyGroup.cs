


namespace TurnFlow.Core.Components;

public class ComponentDependencyGroup
{
    private bool has_changed;

    public ComponentDependencyGroup()
    {
        has_changed = true;
    }

    public void MarkCalculated()
    {
        has_changed = false;
    }

    public void MarkChanged()
    {
        has_changed = true;
    }

    public bool HasChanged()
    {
        return has_changed;
    }
}