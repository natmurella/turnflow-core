

using System;
using TurnFlow.Core.Actions;
using TurnFlow.Core.Characters;
using TurnFlow.Core.Effects;
using TurnFlow.Core.Effects.Plans;
using TurnFlow.Core.Triggers;

namespace TurnFlow.Core.Infos;

public interface IGeneralInfo
{
    // general
    public ITrigger? FromTrigger();
    public IEffect? FromEffect();
    public IAction? FromAction();
    public ICharacter FromCharacter();
    public ICharacter ToCharacter();
}