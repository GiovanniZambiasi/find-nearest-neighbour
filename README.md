# Find Nearest Neighbour

## Neighbour finding logic
Since the brief mentioned that performance should be prioritised, I decided to implement this in
the form of a system. That way, I could control each step of the process and reduce redundancy.

## Later optimizations:

- Reduce `.transform`, `.gameObject` and `.transform.position` calls to get moderate performance
increase;
- Reduce calls to `NeighbourDistanceInfo` constructor for a more desperate optimization
