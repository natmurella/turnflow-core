

using System.Collections.Generic;

namespace TurnFlow.Core.Components.Managers;

public interface IComponentManager
{
    public void Add(string componentName, int value);
    public void Remove(string componentName, int value);
    public int Read(string componentName);
    public List<string> GetEnum(string enumName);
}