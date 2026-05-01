

using TurnFlow.Core.Actions.Plans;
using TurnFlow.Core.Characters;
using TurnFlow.Core.Infos;
using TurnFlow.Core.Managers.Engines;
using TurnFlow.Core.Managers.Handles;
using TurnFlow.Core.Mechanics;

namespace TurnFlow.Core.Actions;

public abstract class Action : IAction
{
    protected CostPlan baseActivateCostPlan;
    protected CostPlan baseEquipCostPlan;
    protected CostPlan baseUnequipCostPlan;
    protected TargetPlan baseTargetPlan;

    protected CostPlan? trueEquipCost;

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
        return CalculateTrueCostPlan(source, baseCost);
    }

    private CostPlan CalculateTrueCostPlan(ICharacter source, CostPlan baseCost)
    {
        return BasicCollectionMechanics.CalculateActionCostPlan(source, baseCost);
    }

    private bool CanPayCost(ICharacter source, CostPlan costPlan)
    {
        return BasicCollectionMechanics.ActionCanPayCost(source, costPlan);
    }

    public bool CanCastTarget(ITriggerEngine engine, ICharacter source, ICharacter target)
    {
        IInfo info = new Info(this, source, target);

        // calc target plan
        engine.Trigger(
            "on_action_targeting_open",
            info
        );
        TargetPlan trueTargetPlan = CalculateTrueTargetPlan(source, baseTargetPlan);
        engine.Trigger(
            "on_action_targeting_close",
            info
        );

        // check if targeting valid
        bool isValidTarget = IsValidTarget(source, target, trueTargetPlan);
        if (!isValidTarget)
        {
            return false;
        }

        // calc cost plan
        engine.Trigger(
            "on_action_cost_open",
            info
        );
        CostPlan trueCost = CalculateTrueCostPlan(source, target, baseActivateCostPlan);
        engine.Trigger(
            "on_action_cost_close",
            info
        );

        // check if cost valid
        bool canPayCost = CanPayCost(source, trueCost);

        return canPayCost;
    }

    private TargetPlan CalculateTrueTargetPlan(ICharacter source, TargetPlan baseTargetPlan)
    {
        // todo
        return baseTargetPlan;
    }

    private bool IsValidTarget(ICharacter source, ICharacter target, TargetPlan targetPlan)
    {
        // todo
        return true;
    }

    private void PayCost(ICharacter source, CostPlan costPlan)
    {
        BasicInteractionMechanics.ApplyActionCost(source, costPlan);
    }

    private void RefundCost(ICharacter source, CostPlan costPlan)
    {
        BasicInteractionMechanics.RefundActionCost(source, costPlan);
    }

    public void Activate(IActionHandle engine, ICharacter source, ICharacter target)
    {
        CostPlan trueCost = CalculateTrueCostPlan(source, target, baseActivateCostPlan);
        PayCost(source, trueCost);

        // todo trigger: on_action_activate_open
        ActivateAction(engine, source, target);
        // todo trigger: on_action_activate_close
    }
    
    public bool IsEquippable(ICharacter source)
    {
        CostPlan trueCost = CalculateTrueCostPlan(source, baseEquipCostPlan);
        return CanPayCost(source, trueCost);
    }

    public void Equip(IActionHandle engine, ICharacter source)
    {
        CostPlan trueCost = CalculateTrueCostPlan(source, baseEquipCostPlan);
        PayCost(source, trueCost);
        trueEquipCost = trueCost;

        // todo trigger: on_action_equip_open
        EquipAction(engine, source);
        // todo trigger: on_action_equip_close
    }

    public bool IsUnequippable(ICharacter source)
    {
        return trueEquipCost != null;
    }

    public void Unequip(IActionHandle engine, ICharacter source)
    {
        if (trueEquipCost == null)
        {
            return;
        }

        CostPlan trueCost = (CostPlan)trueEquipCost;
        RefundCost(source, trueCost);

        // todo trigger: on_action_unequip_open
        UnequipAction(engine, source);
        // todo trigger: on_action_unequip_close
    }

    protected abstract void ActivateAction(IActionHandle engine, ICharacter source, ICharacter target);

    protected abstract void EquipAction(IActionHandle engine, ICharacter source);

    protected abstract void UnequipAction(IActionHandle engine, ICharacter source);
}