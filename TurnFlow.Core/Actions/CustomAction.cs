

using TurnFlow.Core.Actions.Plans;
using TurnFlow.Core.Characters;

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

    protected override  void ActivateAction(ICharacter source, ICharacter target)
    {
        
    }

    protected override void EquipAction(ICharacter source)
    {
        
    }

    protected override void UnequipAction(ICharacter source)
    {
        
    }
}