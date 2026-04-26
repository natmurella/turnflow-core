

using System.Collections.Generic;

namespace TurnFlow.Core.Components.Managers;

public interface IComponentManager
{
    public void Add(string componentName, int value);
    public void Remove(string componentName, int value);
    public void AddToBubble(string componentName, int value, object source, int priority=0);
    public void RemoveFromBubble(string componentName, int value, object source, int priority=0);
    public int Read(string componentName);
    public List<string> GetEnum(string enumName);
}