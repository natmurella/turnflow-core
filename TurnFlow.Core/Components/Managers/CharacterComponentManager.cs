


using System.Collections.Generic;
using System.Text.RegularExpressions;
using TurnFlow.Core.Components.Plans.Processors;
using TurnFlow.Core.Components.Plans.StatPlans;

namespace TurnFlow.Core.Components.Managers;

public class CharacterComponentManager : BaseComponentManager
{
    public CharacterComponentManager(
        StatPlan statPlan,
        BarPlan barPlan,
        ResourcePlan resourcePlan,
        DamageChangePlan damageChangePlan,
        DamageElementPlan damageElementPlan
    ) : base()
    {
        CompileGeneralPlan();
        CompileStatPlan(statPlan);
        CompileBarPlan(barPlan);
        CompileResourcePlan(resourcePlan);
        CompileDamageChangePlan(damageChangePlan);
        CompileDamageElementPlan(damageElementPlan);
        CompileCostModifiers();
    }

    private void CompileGeneralPlan()
    {
        // damage modifiers
        List<string> stat_suffixes = new List<string>
        {
            "add_flat",
            "min_flat",
            "add_mult",
            "min_mult",
        };
        foreach (var suffix in stat_suffixes)
        {
            string componentName = $"damage_{suffix}";
            Component c = new Component();
            components[componentName] = c;
        }
        // set default multiplier to 100%
        this.Add("damage_add_mult", 100);

        // damage change modifiers
        string damageChangeBubble = "damage_change";
        StringBubbleComponent dcb = new StringBubbleComponent();
        this.stringBubbleComponents[damageChangeBubble] = dcb;

        // damage element modifiers
        string damageElementBubble = "damage_element";
        StringBubbleComponent db = new StringBubbleComponent();
        this.stringBubbleComponents[damageElementBubble] = db;
    }

    private void CompileStatPlan(StatPlan statPlan)
    {

        List<string> statEnum = new List<string>();

        foreach (var statDef in statPlan.stats)
        {
            string statName = statDef.statName;
            statEnum.Add(statName);

            ComponentDependencyGroup g = new ComponentDependencyGroup();

            Dictionary<string, string> componentsToUse = new Dictionary<string, string>();
            
            List<string> stat_suffixes = new List<string>
            {
                "add_flat",
                "min_flat",
                "add_mult",
                "min_mult",
            };

            // add simple stat components to dict and group
            foreach (var suffix in stat_suffixes)
            {
                string componentName = $"stat_{statName}_{suffix}";
                Component c = new Component();
                c.AddToGroup(g);
                components[componentName] = c;
                componentsToUse[suffix] = componentName;
            }

            // set default multiplier to 100%
            this.Add($"stat_{statName}_add_mult", 100);

            string secondaryComponentName = $"stat_{statName}";
            StatProcessor sp = new StatProcessor(componentsToUse);
            Component secondaryComponent = new Component(sp, g);
            components[secondaryComponentName] = secondaryComponent;

            // damage modifiers
            string damageSourceModifierPrefix = $"damage_stat_source_{statName}_add_mult";
            components[damageSourceModifierPrefix] = new Component();
        }

        // add enum for stats
        this.enums["stats"] = statEnum;
    }

    private void CompileBarPlan(BarPlan barPlan)
    {

        List<string> barEnum = new List<string>();
        
        foreach (var barDef in barPlan.bars)
        {
            string barName = barDef.barName;

            barEnum.Add(barName);

            string barStatPrefix = $"bar_max_stat_{barName}_";
            ComponentDependencyGroup g = new ComponentDependencyGroup();

            Dictionary<string, string> componentsToUse = new Dictionary<string, string>();
            componentsToUse["prefix"] = barStatPrefix;

            // add prefix
            this.dependencyGroups[barStatPrefix] = g;
            this.sparsePrefixTrie.Insert(barStatPrefix);

            List<string> stat_suffixes = new List<string>
            {
                "add_flat",
                "min_flat",
                "add_mult",
                "min_mult",
            };

            foreach (var suffix in stat_suffixes)
            {
                string componentName = $"bar_max_{barName}_{suffix}";
                Component c = new Component();
                c.AddToGroup(g);
                sparseComponents[componentName] = c;
                componentsToUse[suffix] = componentName;
            }

            // set default multiplier to 100%
            this.Add($"bar_max_{barName}_add_mult", 100);

            // handle flat max value
            if (barDef.maxCalcType == BarMaxCalcType.target)
            {
                this.Add($"bar_max_{barName}_add_flat", barDef.maxValue);
            } 
            // handle sparse stat formulas
            else if (barDef.maxCalcType == BarMaxCalcType.formula)
            {
                // add base value
                this.Add($"bar_max_{barName}_add_flat", barDef.baseBarValue);

                foreach (var formulaDef in barDef.formulaDefs)
                {
                    string sparseBarStatComponentName = $"{barStatPrefix}_{formulaDef.statName}_add_mult";
                    int multiplier = (int)(formulaDef.multiplier * 100);
                    this.Add(sparseBarStatComponentName, multiplier);
                }
            }

            // create current bar value component
            string barComponentName = $"bar_cur_{barName}";
            Component barComponent = new Component();
            components[barComponentName] = barComponent;

            // create bar max value component with processor
            string barMaxComponentName = $"bar_max_{barName}";
            BarProcessor bp = new BarProcessor(componentsToUse);
            Component barMaxComponent = new Component(bp, g);
            components[barMaxComponentName] = barMaxComponent;

            // create bar include bubbles
            string barIncludeBubbleComponentName = $"damage_hit_bar_{barName}_include";
            IntBubbleComponent b = new IntBubbleComponent();
            this.intBubbleComponents[barIncludeBubbleComponentName] = b;
        }

        // add enum for bars
        this.enums["bars"] = barEnum;
    }

    private void CompileResourcePlan(ResourcePlan resourcePlan)
    {
        List<string> resourceEnum = new List<string>();

        foreach (var resourceDef in resourcePlan.resources)
        {
            string resourceName = resourceDef.resourceName;
            string componentName = $"resource_{resourceName}";
            Component c = new Component();
            components[componentName] = c;

            resourceEnum.Add(resourceName);
        }

        // add enum for resources
        this.enums["resources"] = resourceEnum;
    }

    private void CompileDamageChangePlan(DamageChangePlan damageChangePlan)
    {
        
        List<string> damageChangeEnum = new List<string>();

        foreach (var damageChangeDef in damageChangePlan.damageChanges)
        {
            string changeName = damageChangeDef.damageChangeTypeName;
            damageChangeEnum.Add(changeName);

            string damageChangePrefix = $"damage_change_{changeName}_";

            string on_change = $"{damageChangePrefix}on";
            string off_change = $"{damageChangePrefix}off";

            this.Add(on_change, 0);
            this.Add(off_change, 0);
        }

        // add enum for damage change types
        this.enums["damage_changes"] = damageChangeEnum;
    }

    private void CompileDamageElementPlan(DamageElementPlan damageElementPlan)
    {
        List<string> damageElementEnum = new List<string>();

        foreach (var damageElementDef in damageElementPlan.damageElements)
        {
            string elementName = damageElementDef.damageElementName;
            damageElementEnum.Add(elementName);

            string damageElementPrefix = $"damage_element_{elementName}_";

            string on_element = $"{damageElementPrefix}on";
            string off_element = $"{damageElementPrefix}off";

            this.Add(on_element, 0);
            this.Add(off_element, 0);
        }

        // add enum for damage element types
        this.enums["damage_elements"] = damageElementEnum;
    }

    private void CompileCostModifiers()
    {
        // bar cost modifiers
        List<string> bars = this.enums["bars"];
        foreach (var barName in bars)
        {
            string costPrefix = $"bar_cost_{barName}";
            string addFlatBarCost = $"{costPrefix}_add_flat";
            string minFlatBarCost = $"{costPrefix}_min_flat";
            string addMultBarCost = $"{costPrefix}_add_mult";
            string minMultBarCost = $"{costPrefix}_min_mult";

            Component addFlatComponent = new Component();
            Component minFlatComponent = new Component();
            Component addMultComponent = new Component();
            Component minMultComponent = new Component();

            components[addFlatBarCost] = addFlatComponent;
            components[minFlatBarCost] = minFlatComponent;
            components[addMultBarCost] = addMultComponent;
            components[minMultBarCost] = minMultComponent;

            // add default multiplier of 100%
            this.Add(addMultBarCost, 100);
        }

        // resource cost modifiers
        List<string> resources = this.enums["resources"];
        foreach (var resourceName in resources)
        {
            string costPrefix = $"resource_cost_{resourceName}";
            string addFlatResourceCost = $"{costPrefix}_add_flat";
            string minFlatResourceCost = $"{costPrefix}_min_flat";
            string addMultResourceCost = $"{costPrefix}_add_mult";
            string minMultResourceCost = $"{costPrefix}_min_mult";

            Component addFlatComponent = new Component();
            Component minFlatComponent = new Component();
            Component addMultComponent = new Component();
            Component minMultComponent = new Component();

            components[addFlatResourceCost] = addFlatComponent;
            components[minFlatResourceCost] = minFlatComponent;
            components[addMultResourceCost] = addMultComponent;
            components[minMultResourceCost] = minMultComponent;

            // add default multiplier of 100%
            this.Add(addMultResourceCost, 100);
        }
    }
}