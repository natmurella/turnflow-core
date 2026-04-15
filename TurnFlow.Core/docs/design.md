## Design

### Components

The IComponentManager is meant to be a central place to manage fine metrics of a character, like stats, bars, resources, etc. 

I have designed it in a multi-layer fashion, when the bottom layer is the "primary" layer, which are the values that effects cna interact with (ex: current_health, dex_add_flat, etc). Members of the primary layer CANNOT be the target of a dependency group.

The "secondary" layer are values that are calculated by one or more primary (or other secondary) layer members. Unlike primaries, these CANNOT be modified by effects, only calcualted (ex: max_health, dex_value, etc). Members of the secondary MUST be the target of a dependency group.

Dependency groups are a way to only recalculate secondary values when one of their dependencies change. A secondary layer value is the target of a single dependency group, but can be the dependency of multiple groups. A primary layer value can be the dependency of multiple groups, but cannot be the target of any group.

In general, the user of IComponentManager should only read from the secondary layer, and only write to the primary layer. This is not true for things like resources, with not dependencies, these would be read and writable directly in the primary layer.

All primary modifiers CANNOT fall below 0, and must be strictly maintained to not let floating values go unaccounted for. For example, if you have a primary modifier of "health_add_flat", and you apply an effect that adds 10 to it, then you must add 10 to the current value of "health_add_flat". If you later remove that effect, you must subtract 10 from the current value of "health_add_flat". This is to ensure that the current value of "health_add_flat" always reflects the sum of all effects that are currently applied to it.

#### Modifier Names

##### Stats
Primary:
- stat_{statName}_add_flat
- stat_{statName}_min_flat
- stat_{statName}_add_mult
- stat_{statName}_min_mult

secondary:
- stat_{statName}

$$ \text{stat}_{strength} = \text{round}\left( \max(\text{add\_flat}_{strength} - \text{min\_flat}_{strength}, 0) \times \max(\text{add\_mult}_{strength} - \text{min\_mult}_{strength}, 0) \right) $$

##### Bar Currents
Primary:
- bar_cur_{barName}

##### Bar Maxes
Primary:
- bar_max_{barName}_add_flat
- bar_max_{barName}_min_flat
- bar_max_{barName}_add_mult
- bar_max_{barName}_min_mult
Sparse Primary: (prefix: "bar_max_stat_{barName}_")
- bar_max_stat_{barName}_{statName}_add_mult
- bar_max_stat_{barName}_{statName}_min_mult
secondary:
- bar_max_{barName}

$$
\text{bf}_{health} = \max(\text{add\_flat}_{health} - \text{min\_flat}_{health}, 0)
$$
$$
\text{bm}_{health} = \max(\text{add\_mult}_{health} - \text{min\_mult}_{health}, 0)
$$
$$
\text{bs}_{health} = \sum_{\text{stat} \in \text{Stats}} \text{round}( \max(\text{add\_mult}_{health}^{stat} - \text{min\_mult}_{health}^{stat}, 0) \times \text{Stat}_{stat})
$$
$$
\text{bar\_max}_{health} = \text{round}(( \text{bf}_{health} + \text{bs}_{health}) \times \text{bm}_{health} )
$$

##### Bar Costs
Primary:
- bar_cost_{barName}_add_flat
- bar_cost_{barName}_min_flat
- bar_cost_{barName}_add_mult
- bar_cost_{barName}_min_mult
Sparse Primary: (prefix: "bar_cost_force_{barName}_")
- bar_cost_force_{barName}_force_zero
Sparse Primary: (prefix: "bar_cost_bar_{barName}_")
- bar_cost_bar_{barName}_{otherBarName}_add_mult
- bar_cost_bar_{barName}_{otherBarName}_min_mult
Sparse Primary: (prefix: "bar_cost_resource_{barName}_")
- bar_cost_resource_{barName}_{resourceName}_add_mult
- bar_cost_resource_{barName}_{resourceName}_min_mult
secondary:
- bar_cost_{barName}

##### Resources
Primary:
- resource_{resourceName}

##### Resource Costs
Primary:
- resource_cost_{resourceName}_add_flat
- resource_cost_{resourceName}_min_flat
- resource_cost_{resourceName}_add_mult
- resource_cost_{resourceName}_min_mult
Sparse Primary: (prefix: "resource_cost_force_{resourceName}_")
- resource_cost_force_{resourceName}_force_zero
Sparse Primary: (prefix: "resource_cost_resource_{resourceName}_")
- resource_cost_resource_{resourceName}_{otherResourceName}_add_mult
- resource_cost_resource_{resourceName}_{otherResourceName}_min_mult
Sparse Primary: (prefix: "resource_cost_bar_{resourceName}_")
- resource_cost_bar_{resourceName}_{barName}_add_mult
- resource_cost_bar_{resourceName}_{barName}_min_mult
Secondary:
- resource_cost_{resourceName}

#### Enum Names

- "stats"
- "bars"
- "resources"




### Action Hierarchy

Characters interact with the system through discrete actions (ie: turn-based games). 
- actions: can ONLY put effects on the effects stack, and put triggers in the trigger list.
- triggers: can ONLY put effects on the effect stack.
- effects: can ONLY interact with mechanics.
- mechanics: are the ONLY level able to interact with the component manager, and thus change the state of the character.

Effects are independent of each other, and do not pass information between each other within the action.
Instead, use triggers to react to effects.
Example: an attack that heal for damage dealt.
    - DO NOT: have the action do a damage effect, return the damage value, then do a heal effect with that value.
    - DO: have the action set a trigger to heal when damage is dealt, the fire the damage effect. make sure the trigger is scoped to fade on the action's full resolution.
This way the whole system is more flexible to drastic changes is character state.
Effects registered from an Action are resolved on placement of the effectstack. 

Actions have a cost, which must be a resource or current bar value. all other primary and secondary modifiers are OFF LIMITS.
Actions costs cannot give resources/bars (at least not directly). To give resources/bars, use effects on the effect stack.
For Example: if i want to spen my ability points (a resource) on a new skill. that skill's cost would be 1 ability point. But if later i want to reset all skill, refunding all points, I would put not cost on the action unequip, and instead have it give +1 ability point effect.


### Action Modifiers

#### Targeting Modifiers

Defines targeting rules for an action. These are applied at the start of CanCastTarget(), and removed at the end. 
The Exclude modifier, is >= 1 forces exclusion, regardless of number of includes.
If both include and exclude are 0, then the target is excluded by default. In general, ability targeting plans should not apply exclude modifiers (done by opponents instead).

Primary:
- action_target_include_self
- action_target_include_allies
- action_target_include_enemies
- action_target_exclude_self
- action_target_exclude_allies
- action_target_exclude_enemies

#### 

### Trigger Types

#### Action Trigger Types

Costs:
- on_action_cost_open
- on_action_cost_close
Targeting:
- on_action_targeting_open
- on_action_targeting_close
Activate:
- on_action_activate_open
- on_action_activate_close
Equip:
- on_action_equip_open
- on_action_equip_close
Unequip:
- on_action_unequip_open
- on_action_unequip_close


### Damage Change Types

Damage change types define what bars and in what order are affected by damage or healing. They are used in the damage and healing mechanics to determine how to apply the changes to the character's bars.

restricted damage change types:
- cost: only used when paying action costs.

Examples:
- piercing: damage is applied to health, bypassing armor.
- slashing: damage must be applied to armor first, then health.
- magic: damage is applied to magic armor first, then health.

### Damage Element Types

Damage element types define specific damage types that can be affected by bonuses or resistances, without changing the order from damage change type.

restricted damage element types:
- cost: only used when paying action costs.

Examples:
- fire: damage can be reduced by fire resistance, and increased by fire bonuses.
- ice: damage can be reduced by ice resistance, and increased by ice bonuses.