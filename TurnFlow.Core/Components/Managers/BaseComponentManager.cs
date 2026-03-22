

using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace TurnFlow.Core.Components.Managers;


public abstract class BaseComponentManager : IComponentManager
{
    protected Dictionary<string, IComponent> components;
    protected Dictionary<string, IComponent> sparse_components;
    protected Dictionary<string, ComponentDependencyGroup> dependency_groups;
    protected Trie sparse_prefix_trie;
    protected Dictionary<string, List<string>> enums;

    public BaseComponentManager()
    {
        components = new Dictionary<string, IComponent>();
        sparse_components = new Dictionary<string, IComponent>();
        dependency_groups = new Dictionary<string, ComponentDependencyGroup>();
        sparse_prefix_trie = new Trie();
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
        else if (sparse_components.TryGetValue(componentName, out component))
        {
            component.Add(value);
        }
        // check if matching prefix, and add to sparse component if so
        else if (sparse_prefix_trie.FindPrefix(componentName, out prefix))
        {
            ComponentDependencyGroup g;
            if (dependency_groups.TryGetValue(prefix, out g))
            {
                Component c = new Component();
                c.AddToGroup(g);
                c.Add(value);
                sparse_components[componentName] = c;
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
        else if (sparse_components.TryGetValue(componentName, out component))
        {
            bool is_zero = component.Remove(value);
            if (is_zero)
            {
                sparse_components.Remove(componentName);
            }
        }
        else
        {
            throw new KeyNotFoundException($"ComponentManager.Remove: {componentName} not found.");
        }
    }

    public int Read(string componentName)
    {
        IComponent component;
        // if component exists, read from it
        if (components.TryGetValue(componentName, out component))
        {
            return component.Read(this);
        }
        // if sparse component exists, read from it
        else if (sparse_components.TryGetValue(componentName, out component))
        {
            return component.Read(this);
        }
        // if matching prefix, return 0
        else if (sparse_prefix_trie.HasPrefix(componentName))
        {
            return 0;
        }
        else
        {
            throw new KeyNotFoundException($"ComponentManager.Read: {componentName} not found.");
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