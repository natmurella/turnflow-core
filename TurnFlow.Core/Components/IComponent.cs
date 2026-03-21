



namespace TurnFlow.Core.Components;

public interface IComponent
{
    public int Read();
    public void Add(int value);
    public void Remove(int value);
    public void AddToGroup(ComponentDependencyGroup group);
}