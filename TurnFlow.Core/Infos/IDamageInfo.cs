
using System;
using TurnFlow.Core.Actions;
using TurnFlow.Core.Characters;
using TurnFlow.Core.Effects;
using TurnFlow.Core.Effects.Plans;
using TurnFlow.Core.Triggers;

namespace TurnFlow.Core.Infos;

public interface IDamageInfo : IGeneralInfo
{

    // damage info
    public DamageDirectionType GetDamageDirectionType();
    public String GetDamageChangeType();
    public String GetDamageElementType();
    public int GetDamageAmount();
    public string GetDamageBar();
}