

using System;
using TurnFlow.Core.Actions;
using TurnFlow.Core.Characters;
using TurnFlow.Core.Effects;
using TurnFlow.Core.Effects.Plans;
using TurnFlow.Core.Triggers;

namespace TurnFlow.Core.Infos;

public interface IInfo : IGeneralInfo, IDamageInfo
{
    // constructors

    // base
    public void SetTrigger(ITrigger trigger);
    public void SetEffect(IEffect effect);
    public void SetAction(IAction action);
    public void SetFromCharacter(ICharacter fromCharacter);
    public void SetToCharacter(ICharacter toCharacter);

    // damage info
    public void SetDamageDirectionType(DamageDirectionType damageDirectionType);
    public void SetDamageChangeType(String damageChangeType);
    public void SetDamageElementType(String damageElementType);
    public void SetDamageAmount(int damageAmount);
    public void SetDamageBar(string damageBar);

    // namage info null getters
    public string? GetNullDamageBar();
}