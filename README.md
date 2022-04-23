# Find Nearest Neighbour

## Architecture
I wanted to have a very clear hierarchical structure, so that the flow of the application was easily 
understood. I also wanted the core dependencies of each *sub-system* to be very clear for the reader
(as little implicit dependency resolution e.g. singletons or service locators as possible). For those 
reasons, I ended up using a ``Systems/Managers`` based approach. The `System` is a central point of 
communication within the application. It uses ``Managers`` to encapsulate more specific behaviour,
and handles coordination between them. Since the game is very simple, there's only one `System` which controls everything,
called `GameSystem`.

Then, we have four different managers. Each one lives inside its own ``namespace``. Only types that should
be accessible to *every manager* go in the root ``namespace`` (``NearestNeighbour``). This keeps types organized,
grouped by dependency. You can think of each child ``namespace`` as a *sub-system*. In some cases, a particular 
``Manager`` depends upon another `Manager` (this is the case with the ``PoolingManager``, which is a dependency 
of both `NeighbourManager` and `PlayerManager`). When this happens, a public ``interface`` was added to the root 
`namespace`. The managers were then made to depend upon this public ``interface``, instead of the concrete `PoolManager`.
This ensures that no sub-system needs to know about another sub-system. Keeping consistent abstraction between
each "branch" of the application.

## Neighbour finding logic
Since the brief mentioned that performance should be prioritised, I decided to implement the 
neighbour finding logic as a ``Manager``. That way, I could control each step of the process
and avoid redundancy. When the process is centralized within a `Manager`, I'm certain I'm not repeating
distance queries between neighbours that have already been checked. Another upside to using this 
method is that I can break down each individual step, and run them in a particular order, without
having to worry about race-conditions between Unity's different update callbacks.

I've broken down the logic into 3 major steps, which happen in order:
1. ``UpdateMovement``: Moves all cubes in order, and caches their resulting positions;
2. ``UpdateDistances``: Calculates all distances between cubes, updating their nearest neighbours along
the way
3. ``UpdateFeedbacks``: Updates line renderers to conform to the cubes' new positions and nearest neighbours

## Performance

With the current setup, in my machine, and in the editor, the game can handle around 400 cubes while keeping FPS over 60.

Some optimizations steps applied to improve overall performance:
- Caching references to ``Transform`` has proven to be a minor performance increase
- If **even more** performance was needed, some sort of space partitioning algorithim would probably
word well
