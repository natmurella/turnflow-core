

using TurnFlow.Core.Characters;
using TurnFlow.Core.Managers.Engines;
using TurnFlow.Core.Managers.Handles;

namespace TurnFlow.Core.Actions;

public interface IAction
{
    public bool CanCastTarget(ITriggerEngine engine, ICharacter source, ICharacter target);
    public void Activate(IActionHandle engine, ICharacter source, ICharacter target);

    public bool IsEquippable(ICharacter source);
    public void Equip(IActionHandle engine, ICharacter source);

    public bool IsUnequippable(ICharacter source);
    public void Unequip(IActionHandle engine, ICharacter source);
}