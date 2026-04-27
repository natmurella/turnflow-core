using TurnFlow.Core.Components.Managers;

namespace TurnFlow.Core.Components;

public interface IBubbleComponent<T>
{
    public T Read();
    public void Add(T value, object source, int priority=0);
    public bool Remove(T value, object source, int priority=0);
}