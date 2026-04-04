using System;
using System.Text;

namespace TurnFlow.Core.Characters;

public interface ICharacter
{
    public string Name { get; }

    // Optionally, declare a method signature if needed:
    public string GetName();
}