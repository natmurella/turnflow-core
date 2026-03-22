


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
        ResourcePlan resourcePlan
    ) : base()
    {
        CompileStatPlan(statPlan);
        CompileBarPlan(barPlan);
        CompileResourcePlan(resourcePlan);
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
            this.dependency_groups[barStatPrefix] = g;
            this.sparse_prefix_trie.Insert(barStatPrefix);

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
                sparse_components[componentName] = c;
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
        }

        // add enum for bars
        this.enums["bars"] = barEnum;
    }

    private void CompileResourcePlan(ResourcePlan resourcePlan)
    {
        foreach (var resourceDef in resourcePlan.resources)
        {
            string resourceName = resourceDef.resourceName;
            string componentName = $"resource_{resourceName}";
            Component c = new Component();
            components[componentName] = c;
        }
    }
}