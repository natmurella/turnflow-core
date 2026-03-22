
using TurnFlow.Core.Components;
using TurnFlow.Core.Components.Managers;
using TurnFlow.Core.Components.Plans.StatPlans;

namespace TurnFlow.Core.Tests.Components.ComponentManagerTests;


public class ComponentTests
{
    [Fact]
    public void SimpleComponent()
    {
        // define the plans
        StatPlan statPlan = new StatPlan()
        {
            stats = new List<StatDef>()
            {
                new StatDef() { statName = "vitality" },
                new StatDef() { statName = "intelligence" }
            }
        };
        BarPlan barPlan = new BarPlan()
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
        ResourcePlan resourcePlan = new ResourcePlan()
        {
            resources = new List<ResourceDef>()
            {
                new ResourceDef() { resourceName = "gold" },
            }
        };

        CharacterComponentManager ccm = new CharacterComponentManager(statPlan, barPlan, resourcePlan);

        // define the vitality stat
        ccm.Add("stat_vitality_add_flat", 15);
        ccm.Add("stat_vitality_min_flat", 5);
        ccm.Add("stat_vitality_add_mult", 100);
        ccm.Add("stat_vitality_min_mult", 50);

        // check vitality stat calculation
        Assert.Equal(15, ccm.Read("stat_vitality"));

        // define the intelligence stat
        ccm.Add("stat_intelligence_add_flat", 10);
        ccm.Add("stat_intelligence_min_flat", 0);
        ccm.Add("stat_intelligence_add_mult", 50);
        ccm.Add("stat_intelligence_min_mult", 100);

        // check intelligence stat calculation
        Assert.Equal(5, ccm.Read("stat_intelligence"));

        // check health bar max calculation
        Assert.Equal(40, ccm.Read("bar_max_health"));

        // check mana bar max calculation
        Assert.Equal(20, ccm.Read("bar_max_mana"));

        // check stamina bar max calculation
        Assert.Equal(100, ccm.Read("bar_max_stamina"));
    }
}