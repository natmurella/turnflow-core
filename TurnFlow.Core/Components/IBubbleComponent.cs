using TurnFlow.Core.Components.Managers;

namespace TurnFlow.Core.Components;

public interface IBubbleComponent
{
    public int Read();
    public void Add(int value, object source, int priority=0);
    public bool Remove(int value, object source, int priority=0);
}