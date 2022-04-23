# Find Nearest Neighbour

## Neighbour finding logic
Since the brief mentioned that performance should be prioritised, I decided to implement this in
the form of a system. That way, I could control each step of the process and reduce redundancy.
When the process is centralized in a `Manager`, I'm certain I'm not repeating distance queries
between neighbours that have already been checked. Another upside to using this method is that I can break down each individual step, and run them in
a particular order, without having to worry about race-conditions between Unity's different update
callbacks.

I've broken down the logic into 3 major steps, which happen in order:
1. ``UpdateMovement``: Moves all cubes in order, and caches their resulting positions;
2. ``UpdateDistances``: Calculates all distances between cubes, updating their nearest neighbour along
the way
3. ``UpdateFeedbacks``: Updates line renderers to conform to the cube's new positions and nearest neighbours

## Later optimizations:

- Reduce `.transform`, `.gameObject` and `.transform.position` calls to get moderate performance
increase;
- Reduce calls to `NeighbourDistanceInfo` constructor for a more desperate optimization
- If **even more** performance was needed, some sort of space partitioning algorithim would probably
word well

## TODO:
- [ ] Spawn with keys?
- [X] Player shooting
- [ ] Shader effects for fun?
- [X] Shooting Statistics
