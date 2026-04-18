

using System.Collections.Generic;
using TurnFlow.Core.Actions;
using TurnFlow.Core.Characters;
using TurnFlow.Core.Infos;
using TurnFlow.Core.Managers.Engines;
using TurnFlow.Core.Managers.Handles;
using TurnFlow.Core.Mechanics;
using TurnFlow.Core.Triggers;

namespace TurnFlow.Core.Effects.BasicEffects;

public abstract class StaticBarChangeEffect : Effect
{
    protected int damageAmount;
    protected string damageChangeType;
    protected string damageElementType;

    public StaticBarChangeEffect(
        IAction action,
        ICharacter source, 
        ICharacter target, 
        int damageAmount,
        string damageChangeType,
        string damageElementType
    ) : base(
            action,
            source, 
            target
        )
    {
        this.damageAmount = damageAmount;
        this.damageChangeType = damageChangeType;
        this.damageElementType = damageElementType;
    }

    protected override void Execute(IEffectHandle engine, IInfo info)
    {
        List<TriggerParams> triggers = BasicInteractionMechanics.DealDamageOrHeal(
            info
        );

        engine.TriggerAll(triggers);
    }
}