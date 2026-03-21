## Design

### Components

The IComponentManager is meant to be a central place to manage fine metrics of a character, like stats, bars, resources, etc. 

I have designed it in a multi-layer fashion, when the bottom layer is the "primary" layer, which are the values that effects cna interact with (ex: current_health, dex_add_flat, etc). Members of the primary layer CANNOT be the target of a dependency group.

The "secondary" layer are values that are calculated by one or more primary (or other secondary) layer members. Unlike primaries, these CANNOT be modified by effects, only calcualted (ex: max_health, dex_value, etc). Members of the secondary MUST be the target of a dependency group.

Dependency groups are a way to only recalculate secondary values when one of their dependencies change. A secondary layer value is the target of a single dependency group, but can be the dependency of multiple groups. A primary layer value can be the dependency of multiple groups, but cannot be the target of any group.

In general, the user of IComponentManager should only read from the secondary layer, and only write to the primary layer. This is not true for things like resources, with not dependencies, these would be read and writable directly in the primary layer.

All priamry modifiers CANNOT fall below 0, and must be strictly maintained to not let floating values go unaccounted for. For example, if you have a primary modifier of "health_add_flat", and you apply an effect that adds 10 to it, then you must add 10 to the current value of "health_add_flat". If you later remove that effect, you must subtract 10 from the current value of "health_add_flat". This is to ensure that the current value of "health_add_flat" always reflects the sum of all effects that are currently applied to it.