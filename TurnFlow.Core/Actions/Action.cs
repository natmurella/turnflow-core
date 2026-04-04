

using TurnFlow.Core.Actions.Plans;
using TurnFlow.Core.Characters;

namespace TurnFlow.Core.Actions;

public abstract class Action : IAction
{
    protected CostPlan baseActivateCostPlan;
    protected CostPlan baseEquipCostPlan;
    protected CostPlan baseUnequipCostPlan;
    protected TargetPlan baseTargetPlan;

    public Action(
        CostPlan activateCost, 
        CostPlan equipCost, 
        CostPlan unequipCost, 
        TargetPlan targetPlan
    ) {
        this.baseActivateCostPlan = activateCost;
        this.baseEquipCostPlan = equipCost;
        this.baseUnequipCostPlan = unequipCost;
        this.baseTargetPlan = targetPlan;
    }

    private CostPlan CalculateTrueCostPlan(ICharacter source, ICharacter target, CostPlan baseCost)
    {
        // todo trigger: on_action_cost_open
        // todo
        return baseCost;

        // todo trigger: on_action_cost_close
    }

    private CostPlan CalculateTrueCostPlan(ICharacter source, CostPlan baseCost)
    {
        // todo trigger: on_action_cost_open
        // todo
        return baseCost;

        // todo trigger: on_action_cost_close
    }

    private bool CanPayCost(ICharacter source, CostPlan costPlan)
    {
        // todo
        return true;
    }

    public bool CanCastTarget(ICharacter source, ICharacter target)
    {
        TargetPlan trueTargetPlan = CalculateTrueTargetPlan(source, baseTargetPlan);
        bool isValidTarget = IsValidTarget(source, target, trueTargetPlan);

        if (!isValidTarget)
        {
            return false;
        }

        CostPlan trueCost = CalculateTrueCostPlan(source, target, baseActivateCostPlan);
        return CanPayCost(source, trueCost);
    }

    private TargetPlan CalculateTrueTargetPlan(ICharacter source, TargetPlan baseTargetPlan)
    {
        // todo trigger: on_action_targeting_open
        // todo
        return baseTargetPlan;
        
        // todo trigger: on_action_targeting_close
    }

    private bool IsValidTarget(ICharacter source, ICharacter target, TargetPlan targetPlan)
    {
        // todo
        return true;
    }

    private void PayCost(ICharacter source, CostPlan costPlan)
    {
        // todo
    }

    public void Activate(ICharacter source, ICharacter target)
    {
        CostPlan trueCost = CalculateTrueCostPlan(source, target, baseActivateCostPlan);
        PayCost(source, trueCost);

        // todo trigger: on_action_activate_open
        ActivateAction(source, target);
        // todo trigger: on_action_activate_close
    }
    
    public bool IsEquippable(ICharacter source)
    {
        CostPlan trueCost = CalculateTrueCostPlan(source, baseEquipCostPlan);
        return CanPayCost(source, trueCost);
    }

    public void Equip(ICharacter source)
    {
        CostPlan trueCost = CalculateTrueCostPlan(source, baseEquipCostPlan);
        PayCost(source, trueCost);

        // todo trigger: on_action_equip_open
        EquipAction(source);
        // todo trigger: on_action_equip_close
    }

    public bool IsUnequippable(ICharacter source)
    {
        CostPlan trueCost = CalculateTrueCostPlan(source, baseUnequipCostPlan);
        return CanPayCost(source, trueCost);
    }

    public void Unequip(ICharacter source)
    {
        CostPlan trueCost = CalculateTrueCostPlan(source, baseUnequipCostPlan);
        PayCost(source, trueCost);

        // todo trigger: on_action_unequip_open
        UnequipAction(source);
        // todo trigger: on_action_unequip_close
    }

    protected abstract void ActivateAction(ICharacter source, ICharacter target);

    protected abstract void EquipAction(ICharacter source);

    protected abstract void UnequipAction(ICharacter source);
}