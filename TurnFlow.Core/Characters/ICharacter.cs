using System;
using System.ComponentModel;
using System.Text;
using TurnFlow.Core.Components.Managers;

namespace TurnFlow.Core.Characters;

public interface ICharacter
{
    public string GetName();
    public IComponentManager GetComponentManager();
}