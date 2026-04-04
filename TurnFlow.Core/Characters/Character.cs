using System;
using System.Text;

namespace TurnFlow.Core.Characters;



public class Character : ICharacter
{
    public string Name { get; private set; }

    public Character(string name)
    {
        Name = name;
    }

    public string GetName()
    {
        return Name;
    }
}