
using TurnFlow.Core.Actions;
using TurnFlow.Core.Actions.Plans;
using TurnFlow.Core.Characters;
using TurnFlow.Core.Components;
using TurnFlow.Core.Components.Managers;
using TurnFlow.Core.Components.Plans.StatPlans;
using TurnFlow.Core.Effects.BasicEffects;
using TurnFlow.Core.Effects.Plans;
using TurnFlow.Core.Managers.Engines;

namespace TurnFlow.Core.Tests.Actions.DamageActionTests;


public static class SimplePlans
{
    public static StatPlan GetStatPlan()
    {
        return new StatPlan()
        {
            stats = new List<StatDef>()
            {
                new StatDef() { statName = "vitality" },
                new StatDef() { statName = "intelligence" }
            }
        };
    }

    public static BarPlan GetBarPlan()
    {
        return new BarPlan()
        {
            bars = new List<BarDef>()
            {
                new BarDef() { 
                    barName = "health",
                    resetType = BarResetType.toMaximum,
                    maxCalcType = BarMaxCalcType.formula,
                    baseBarValue = 10,
                    formulaDefs = new List<BarFormulaDef>()
                    {
                        new BarFormulaDef()
                        {
                            statName = "vitality",
                            multiplier = 2.0f,
                        }
                    }
                },
                new BarDef()
                {
                    barName = "mana",
                    resetType = BarResetType.toMaximum,
                    maxCalcType = BarMaxCalcType.formula,
                    baseBarValue = 5,
                    formulaDefs = new List<BarFormulaDef>()
                    {
                        new BarFormulaDef()
                        {
                            statName = "intelligence",
                            multiplier = 3.0f,
                        }
                    }
                },
                new BarDef()
                {
                    barName = "stamina",
                    resetType = BarResetType.toMaximum,
                    maxCalcType = BarMaxCalcType.target,
                    maxValue = 100,
                }
            }
        };
    }

    public static ResourcePlan GetResourcePlan()
    {
        return new ResourcePlan()
        {
            resources = new List<ResourceDef>()
            {
                new ResourceDef() { resourceName = "gold" },
            }
        };
    }

    public static DamageChangePlan GetDamageChangePlan()
    {
        return new DamageChangePlan()
        {
            damageChanges = new List<DamageChangeDef>()
            {
                new DamageChangeDef()
                {
                    damageChangeTypeName = "life",
                    barOrder = new List<DamageChangeBarDef>()
                    {
                        new DamageChangeBarDef() { barName = "health" },
                    }
                },
                new DamageChangeDef()
                {
                    damageChangeTypeName = "soul",
                    barOrder = new List<DamageChangeBarDef>()
                    {
                        new DamageChangeBarDef() { barName = "mana" },
                    }
                },
                new DamageChangeDef()
                {
                    damageChangeTypeName = "pure",
                    barOrder = new List<DamageChangeBarDef>()
                    {
                        new DamageChangeBarDef() { barName = "health" },
                        new DamageChangeBarDef() { barName = "mana" },
                        new DamageChangeBarDef() { barName = "stamina" },
                    }
                }
            }
        };
    }

    public static DamageElementPlan GetDamageElementPlan()
    {
        return new DamageElementPlan()
        {
            damageElements = new List<DamageElementDef>()
            {
                new DamageElementDef() { damageElementName = "physical" },
                new DamageElementDef() { damageElementName = "fire" },
                new DamageElementDef() { damageElementName = "ice" },
            }
        };
    }

    public static CostPlan GetActivateCostPlan()
    {
        return new CostPlan()
        {
            barCosts = new List<BarCostDef>()
            {
                new BarCostDef() { barName = "mana", costValue = 1 },
            },
            resourceCosts = new List<ResourceCostDef>()
            {
                
            }
        };
    }

    public static CostPlan GetEquipCostPlan()
    {
        return new CostPlan()
        {
            barCosts = new List<BarCostDef>()
            {
                
            },
            resourceCosts = new List<ResourceCostDef>()
            {
                new ResourceCostDef() { resourceName = "gold", costValue = 1 },
            }
        };
    }

    public static CostPlan GetUnequipCostPlan()
    {
        return new CostPlan()
        {
            barCosts = new List<BarCostDef>()
            {
                
            },
            resourceCosts = new List<ResourceCostDef>()
            {
                new ResourceCostDef() { resourceName = "gold", costValue = 1 },
            }
        };
    }

    public static TargetPlan GetTargetPlan()
    {
        return new TargetPlan()
        {
            canTargetSelf = true,
            canTargetAllies = true,
            canTargetEnemies = true,
        };
    }

    private static DamageEffectPlan GetDamageEffectPlan()
    {
        return new DamageEffectPlan()
        {
            damageChangeTypeName = "life",
            damageElementName = "physical",
            damageDirectionType = DamageDirectionType.Damage,
            baseAmount = 5,
            statSourceScalingDict = new Dictionary<string, int>()
            {
                { "vitality", 2 },
                // { "intelligence", 1 },
            }
        };
    }

    public static CustomPlan GetCustomPlan()
    {
        return new CustomPlan()
        {
            isDamage = true,
            damageEffectPlan = GetDamageEffectPlan(),
            isBuff = false,
            buffDef = null,
            isDebuff = false,
            debuffDef = null,
        };
    }
}







public class DamageActionTests
{
    [Fact]
    public void DamageActionTest()
    {
        // define plans
        StatPlan statPlan = SimplePlans.GetStatPlan();
        BarPlan barPlan = SimplePlans.GetBarPlan();
        ResourcePlan resourcePlan = SimplePlans.GetResourcePlan();
        DamageChangePlan damageChangePlan = SimplePlans.GetDamageChangePlan();
        DamageElementPlan damageElementPlan = SimplePlans.GetDamageElementPlan();

        // define component managers
        CharacterComponentManager ccm1 = new CharacterComponentManager(
            statPlan, 
            barPlan, 
            resourcePlan,
            damageChangePlan,
            damageElementPlan
        );
        CharacterComponentManager ccm2 = new CharacterComponentManager(
            statPlan, 
            barPlan, 
            resourcePlan,
            damageChangePlan,
            damageElementPlan
        );

        // define characters
        Character c1 = new Character("c1", ccm1);
        Character c2 = new Character("c2", ccm2);

        // define trigger engine
        TriggerEngine te = new TriggerEngine();
        te.SetupSystemTriggers(damageChangePlan);

        // define damage action
        CustomAction a1 = new CustomAction(
            SimplePlans.GetActivateCostPlan(),
            SimplePlans.GetEquipCostPlan(),
            SimplePlans.GetUnequipCostPlan(),
            SimplePlans.GetTargetPlan(),
            SimplePlans.GetCustomPlan()
        );

        // set characters vit and int
        ccm1.Add("stat_vitality_add_flat", 5);
        ccm1.Add("stat_intelligence_add_flat", 10);
        ccm2.Add("stat_vitality_add_flat", 5);
        ccm2.Add("stat_intelligence_add_flat", 10);

        // check initial health and mana
        Assert.Equal(20, ccm1.Read<int>("bar_max_health"));
        Assert.Equal(35, ccm1.Read<int>("bar_max_mana"));
        Assert.Equal(20, ccm2.Read<int>("bar_max_health"));
        Assert.Equal(35, ccm2.Read<int>("bar_max_mana"));

        // reset character bars
        c1.ResetBars();
        c2.ResetBars();

        // check current health and mana
        Assert.Equal(20, ccm1.Read<int>("bar_cur_health"));
        Assert.Equal(35, ccm1.Read<int>("bar_cur_mana"));
        Assert.Equal(20, ccm2.Read<int>("bar_cur_health"));
        Assert.Equal(35, ccm2.Read<int>("bar_cur_mana"));

        // activate damage action from c1 to c2
        a1.Activate(te, c1, c2);

        // check health after damage        
        Assert.Equal(5, ccm2.Read<int>("bar_cur_health"));
    }

}