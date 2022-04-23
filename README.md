# Find Nearest Neighbour

https://user-images.githubusercontent.com/46461122/164943529-ecd152f4-33d9-484c-bc18-f8ad5864c78c.mp4

## Architecture
I wanted to have a very clear hierarchical structure, so that the flow of the application was easily 
understood. I also wanted the core dependencies of each *sub-system* to be very clear for the reader
(as little implicit dependency resolution e.g. singletons or service locators as possible). For those 
reasons, I ended up using a ``Systems/Managers`` based approach. The `System` is a central point of 
communication within the application. It uses ``Managers`` to encapsulate more specific behaviour,
and handles coordination between them. Since the game is very simple, there's only one `System` which controls everything,
called `GameSystem`. Here's an overview:

![Architecture Overview](https://user-images.githubusercontent.com/46461122/164943352-fd12a4fd-7a1b-4ae2-8dd6-ea16260884f8.png)  
*Architecture Overview*

Then, we have four different managers. Each one lives inside its own ``namespace``. Only types that should
be accessible to *every manager* go in the root ``namespace`` (``NearestNeighbour``). This keeps types organized,
grouped by dependency. You can think of each child ``namespace`` as a *sub-system*. In some cases, a particular 
``Manager`` depends upon another `Manager` (this is the case with the ``PoolingManager``, which is a dependency 
of both `NeighbourManager` and `PlayerManager`). When this happens, a public ``interface`` was added to the root 
`namespace`. The managers were then made to depend upon this public ``interface``, instead of the concrete `PoolManager`.
This ensures that no sub-system needs to know about another sub-system. Keeping consistent abstraction between
each "branch" of the application.

![System Boundaries](https://user-images.githubusercontent.com/46461122/164943370-acd5e1a1-1c1b-4790-aa09-0916aefd93c4.png)  
*System boundaries (defined by namespace)*

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

As feedback for when the cubes change direction, I've implemented a quick emissive blink in shader graph:

https://user-images.githubusercontent.com/46461122/164943474-287a3409-acc2-4a26-9087-9da7d46f373d.mp4

## Performance

With the current setup, in my machine, and in the editor, the game can handle around 400 cubes while keeping FPS over 60:

https://user-images.githubusercontent.com/46461122/164943560-3a8df4b5-ea9e-453b-a8cd-6ea880f40ab6.mp4

Some notes about optimization:
- Caching references to ``Transform`` has proven to be a minor performance increase;
- Using `.localPosition` instead of `.position` has improved performance by about 22%;
- No calls were made to ``Vector3.magnitude`` to avoid square root calculations;
- A utility class called `GameObjectComponentCache` has been implemented to avoid having to call `.GetComponent` repeatedly;
- If **even more** performance was needed, some sort of space partitioning algorithim would likely be my next attempt;
- With user experience in mind, I've set a hard-limit of 1000 cubes. This can be changed in the `NeighbourManager` inspector. 
*(Crashing Unity after **accidentally** trying to spawn 999999 cubes was getting annoying 🤯)*
