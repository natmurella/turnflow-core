

using System;
using TurnFlow.Core.Actions;
using TurnFlow.Core.Characters;
using TurnFlow.Core.Effects;
using TurnFlow.Core.Triggers;

namespace TurnFlow.Core.Infos;

public interface IInfo
{

    public ITrigger? FromTrigger();
    public IEffect? FromEffect();
    public IAction? FromAction();
    public ICharacter? FromCharacter();

    public int GetDetails(String deatilName);
}