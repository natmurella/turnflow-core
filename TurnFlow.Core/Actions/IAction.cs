

using TurnFlow.Core.Characters;

namespace TurnFlow.Core.Actions;

public interface IAction
{
    public bool CanCastTarget(ICharacter source, ICharacter target);
    public void Activate(ICharacter source, ICharacter target);

    public bool IsEquippable(ICharacter source);
    public void Equip(ICharacter source);

    public bool IsUnequippable(ICharacter source);
    public void Unequip(ICharacter source);
}