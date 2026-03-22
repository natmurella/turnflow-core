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

##### Resources
Primary:
- resource_{resourceName}


#### Enum Names

- "stats"
- "bars"
- "resources"