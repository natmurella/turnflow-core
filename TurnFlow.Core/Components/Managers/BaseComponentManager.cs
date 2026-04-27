

using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using TurnFlow.Core.Components.Plans.StatPlans;

namespace TurnFlow.Core.Components.Managers;


public abstract class BaseComponentManager : IComponentManager
{
    protected Dictionary<string, IComponent> components;
    protected Dictionary<string, IComponent> sparseComponents;
    protected Dictionary<string, ComponentDependencyGroup> dependencyGroups;
    protected Dictionary<string, IBubbleComponent<int>> intBubbleComponents;
    protected Dictionary<string, IBubbleComponent<string>> stringBubbleComponents;
    protected Trie sparsePrefixTrie;
    protected Dictionary<string, List<string>> enums;

    public BaseComponentManager()
    {
        components = new Dictionary<string, IComponent>();
        sparseComponents = new Dictionary<string, IComponent>();
        dependencyGroups = new Dictionary<string, ComponentDependencyGroup>();
        intBubbleComponents = new Dictionary<string, IBubbleComponent<int>>();
        stringBubbleComponents = new Dictionary<string, IBubbleComponent<string>>();
        sparsePrefixTrie = new Trie();
        enums = new Dictionary<string, List<string>>();
    }

    public void Add(string componentName, int value)
    {

        if (value == 0)
        {
            return;
        }

        IComponent component;
        string prefix;
        // add to components if it exists
        if (components.TryGetValue(componentName, out component))
        {
            component.Add(value);
        } 
        // add to sparse components if it exists
        else if (sparseComponents.TryGetValue(componentName, out component))
        {
            component.Add(value);
        }
        // check if matching prefix, and add to sparse component if so
        else if (sparsePrefixTrie.FindPrefix(componentName, out prefix))
        {
            ComponentDependencyGroup g;
            if (dependencyGroups.TryGetValue(prefix, out g))
            {
                Component c = new Component();
                c.AddToGroup(g);
                c.Add(value);
                sparseComponents[componentName] = c;
            }
            else
            {
                throw new KeyNotFoundException($"ComponentManager.Add: Dependency group for prefix {prefix} not found.");
            }
        }
        else
        {
            throw new KeyNotFoundException($"ComponentManager.Add: {componentName} not found.");
        }
    }

    public void Remove(string componentName, int value)
    {
        IComponent component;
        // if component exists, remove from it
        if (components.TryGetValue(componentName, out component))
        {
            component.Remove(value);
        }
        // if sparse component exists, remove from it. remove from sparse if 0 after removal.
        else if (sparseComponents.TryGetValue(componentName, out component))
        {
            bool is_zero = component.Remove(value);
            if (is_zero)
            {
                sparseComponents.Remove(componentName);
            }
        }
        else
        {
            throw new KeyNotFoundException($"ComponentManager.Remove: {componentName} not found.");
        }
    }

    public void AddToBubble<T>(string componentName, T value, object source, int priority=0)
    {
        
        if (value is int intValue)
        {
            IBubbleComponent<int> intBubbleComponent;
            if (this.intBubbleComponents.TryGetValue(componentName, out intBubbleComponent))
            {
                intBubbleComponent.Add(intValue, source, priority);
            }
            else
            {
                throw new KeyNotFoundException($"ComponentManager.AddToBubble: {componentName} not found.");
            }
        }
        else if (value is string strValue)
        {
            IBubbleComponent<string> stringBubbleComponent;
            if (this.stringBubbleComponents.TryGetValue(componentName, out stringBubbleComponent))
            {
                stringBubbleComponent.Add(strValue, source, priority);
            }
            else
            {
                throw new KeyNotFoundException($"ComponentManager.AddToBubble: {componentName} not found.");
            }
        }
        else
        {
            throw new ArgumentException($"ComponentManager.AddToBubble: Unsupported bubble component type {typeof(T)}.");
        }
        
    }

    public void RemoveFromBubble<T>(string componentName, T value, object source, int priority=0)
    {
        if (value is int intValue)
        {
            IBubbleComponent<int> intBubbleComponent;
            if (this.intBubbleComponents.TryGetValue(componentName, out intBubbleComponent))
            {
                intBubbleComponent.Remove(intValue, source, priority);
            }
            else
            {
                throw new KeyNotFoundException($"ComponentManager.RemoveFromBubble: {componentName} not found.");
            }
        }
        else if (value is string strValue)
        {
            IBubbleComponent<string> stringBubbleComponent;
            if (this.stringBubbleComponents.TryGetValue(componentName, out stringBubbleComponent))
            {
                stringBubbleComponent.Remove(strValue, source, priority);
            }
            else
            {
                throw new KeyNotFoundException($"ComponentManager.RemoveFromBubble: {componentName} not found.");
            }
        }
        else
        {
            throw new ArgumentException($"ComponentManager.RemoveFromBubble: Unsupported bubble component type {typeof(T)}.");
        }
     }

    public T Read<T>(string componentName)
    {
        IComponent component;

        // if component exists, read from it
        if (components.TryGetValue(componentName, out component))
        {
            return (T)(object)component.Read(this);
        }
        // if sparse component exists, read from it
        else if (sparseComponents.TryGetValue(componentName, out component))
        {
            return (T)(object)component.Read(this);
        }
        // if matching prefix, return 0
        else if (sparsePrefixTrie.HasPrefix(componentName))
        {
            return (T)(object)0;
        }
        else if (this.intBubbleComponents.TryGetValue(componentName, out var intBubbleComponent))
        {
            return (T)(object)intBubbleComponent.Read();
        }
        else if (this.stringBubbleComponents.TryGetValue(componentName, out var stringBubbleComponent))
        {
            return (T)(object)stringBubbleComponent.Read();
        }
        else
        {
            throw new KeyNotFoundException($"ComponentManager.Read: {componentName} not found.");
        }
    }

    public void ResetBar(string barName, BarResetType barResetType)
    {
        string barCurName = $"bar_cur_{barName}";
        string barMaxName = $"bar_max_{barName}";

        if (barResetType == BarResetType.toMaximum)
        {
            int barMax = Read<int>(barMaxName);
            int barCur = Read<int>(barCurName);
            if (barCur <= barMax)
            {
                Add(barCurName, barMax - barCur);
            }
            else
            {
                Remove(barCurName, barCur - barMax);
            }
        }
        else if (barResetType == BarResetType.toMinimum)
        {
            int barCur = Read<int>(barCurName);
            if (barCur > 0)
            {
                Remove(barCurName, barCur);
            }
        }
        else if (barResetType == BarResetType.noChange)
        {
            
        }
        else
        {
            throw new InvalidDataException($"ComponentManager.ResetBar: Invalid BarResetType {barResetType}.");
        }
    }

    public List<string> GetEnum(string enumName)
    {
        List<string> enumValues;
        if (enums.TryGetValue(enumName, out enumValues))
        {
            return enumValues;
        }
        else
        {
            throw new KeyNotFoundException($"ComponentManager.GetEnum: {enumName} not found.");
        }
    }
}