



using TurnFlow.Core.Components.Managers;

namespace TurnFlow.Core.Components;

public interface IComponent
{
    public int Read(IComponentManager componentManager);
    public void Add(int value);
    public bool Remove(int value);
    public void AddToGroup(ComponentDependencyGroup group);
}