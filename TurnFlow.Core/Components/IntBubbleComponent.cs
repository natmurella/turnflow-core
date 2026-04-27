
using System;
using System.Collections.Generic;
using TurnFlow.Core.Components.Managers;
using TurnFlow.Core.Components.Plans.Processors;

namespace TurnFlow.Core.Components;


public class IntBubbleComponent : IBubbleComponent<int>
{
    private int value;
    private SortedSet<(int priority, long timestamp, int id, int value)> sources;
    private Dictionary<(object source, int priority, int value), List<(int id, long timestamp)>> sourceIds;
    private int nextId;

    private int getNextId()
    {
        return nextId++;
    }

    public IntBubbleComponent()
    {
        this.value = 0;
        this.sources = new SortedSet<(int priority, long timestamp, int id, int value)>();
        this.sourceIds = new Dictionary<(object source, int priority, int value), List<(int id, long timestamp)>>();
        this.nextId = 0;
    }

    public int Read()
    {
        return this.value;
    }

    public void Add(int value, object source, int priority=0)
    {
        if (source == null)
        {
            throw new ArgumentNullException(nameof(source), "BubbleComponent.Add: source cannot be null.");
        }

        int id = getNextId();
        long ts = DateTime.UtcNow.Ticks;

        sources.Add((priority, ts, id, value));

        var key = (source, priority, value);
        if (!sourceIds.ContainsKey(key))
        {
            sourceIds[key] = new List<(int id, long timestamp)>();
        }
        sourceIds[key].Add((id, ts));

        this.value = sources.Max.value;
    }

    // returns whether the final values is 0 or not.
    public bool Remove(int value, object source, int priority=0)
    {
        if (source == null)
        {
            throw new ArgumentNullException(nameof(source), "BubbleComponent.Remove: source cannot be null.");
        }

        var key = (source, priority, value);
        if (!sourceIds.ContainsKey(key))
        {
            return false;
        }

        List<(int id, long timestamp)> ids = sourceIds[key];
        (int id, long ts) = ids[0];
        ids.RemoveAt(0);
        if (ids.Count == 0)
        {
            sourceIds.Remove(key);
        }

        sources.Remove((priority, ts, id, value));
        if (sources.Count == 0)
        {
            this.value = 0;
            return true;
        }
        else
        {
            this.value = sources.Max.value;
            return false;
        }
    }
}