using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using TurnFlow.Core.Components.Managers;
using TurnFlow.Core.Components.Plans.StatPlans;

namespace TurnFlow.Core.Characters;



public class Character : ICharacter
{
    public string name { get; private set; }
    public IComponentManager componentManager;

    public Character(string name, IComponentManager componentManager)
    {
        this.componentManager = componentManager;
        this.name = name;
    }

    public Character(IComponentManager componentManager)
    {
        this.componentManager = componentManager;
        this.name = "unknown";
    }

    public string GetName()
    {
        return this.name;
    }

    public void ResetBars()
    {
        List<string> bars = componentManager.GetEnum("bars");
        foreach (string bar in bars)
        {
            componentManager.ResetBar(bar, BarResetType.toMaximum);
        }
    }

    public IComponentManager GetComponentManager()
    {
        return this.componentManager;
    }
}