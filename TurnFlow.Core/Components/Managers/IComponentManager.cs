

using System.Collections.Generic;
using TurnFlow.Core.Components.Plans.StatPlans;

namespace TurnFlow.Core.Components.Managers;

public interface IComponentManager
{
    public void Add(string componentName, int value);
    public void Remove(string componentName, int value);
    public void AddToBubble<T>(string componentName, T value, object source, int priority=0);
    public void RemoveFromBubble<T>(string componentName, T value, object source, int priority=0);
    public T Read<T>(string componentName);
    public void ResetBar(string barName, BarResetType barResetType);
    public List<string> GetEnum(string enumName);
}