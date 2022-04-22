# Find Nearest Neighbour

## Neighbour finding logic
Since the brief mentioned that performance should be prioritised, I decided to implement this in
the form of a system. That way, I could control each step of the process and reduce redundancy.

## `NeighbourCache`
This is a shared cache of all the shortest distances between two `GameObjects`. For each neighbour,
an entry of `NeighbourDistanceCache` is created, and added to a `Dictionary` which maps the data to
a particular `GameObject`.
