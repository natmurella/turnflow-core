

using TurnFlow.Core.Actions.Plans;
using TurnFlow.Core.Characters;
using TurnFlow.Core.Effects.BasicEffects;
using TurnFlow.Core.Effects.Plans;
using TurnFlow.Core.Managers.Handles;

namespace TurnFlow.Core.Actions;


public class CustomAction : Action
{
    protected CustomPlan baseCustomPlan;

    public CustomAction(
        CostPlan activateCost, 
        CostPlan equipCost, 
        CostPlan unequipCost, 
        TargetPlan targetPlan,
        CustomPlan customPlan
    ) : base(activateCost, equipCost, unequipCost, targetPlan)
    {
        baseCustomPlan = customPlan;
    }

    protected override  void ActivateAction(IActionHandle engine, ICharacter source, ICharacter target)
    {
        if (baseCustomPlan.isDamage && baseCustomPlan.damageEffectPlan is DamageEffectPlan dep)
        {
            DamageEffect damageEffect = new DamageEffect(this, source, target, dep);
            engine.RegisterEffect(damageEffect);
        }
    }

    protected override void EquipAction(IActionHandle engine, ICharacter source)
    {
        // todo
    }

    protected override void UnequipAction(IActionHandle engine, ICharacter source)
    {
        // todo
    }
}