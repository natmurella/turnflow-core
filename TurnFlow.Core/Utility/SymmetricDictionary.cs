
using System.Collections.Generic;
using TurnFlow.Core.Characters;
using TurnFlow.Core.Infos;

namespace TurnFlow.Core.Utility;

public class SymmetricDictionary<K, V>
{
    protected Dictionary<K, V> pairs;
    protected HashSet<V> values;

    public SymmetricDictionary()
    {
        pairs = new Dictionary<K, V>();
        values = new HashSet<V>();
    }

    public void Add(K key, V value)
    {
        pairs.Add(key, value);
        values.Add(value);
    }

    public void Remove(K key)
    {
        if (pairs.TryGetValue(key, out V value))
        {
            pairs.Remove(key);
            values.Remove(value);
        }
    }

    public V GetValue(K key)
    {
        if (pairs.TryGetValue(key, out V value))
        {
            return value;
        }
        else
        {
            throw new KeyNotFoundException($"SymmetricDictionary.GetValue: Key {key} not found.");
        }
    }

    public HashSet<V> GetValues()
    {
        return values;
    }
}