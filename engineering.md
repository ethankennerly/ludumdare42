# Engineering Features

- [ ] TODO
- [x] Done

## Motion

### Editor flow

1. [x] Pause.
    1. [x] Tap button to start.
    1. [x] Read instruction.
1. [x] See [Mover System](LudumDare42/Assets/Scripts/MoverSystem.cs)
    1. [x] See a block.
    1. [x] Move blocks at configurable speed.
    1. [x] Tilt camera to see maximum blocks.
1. [x] Mover Steering Listener
    1. [x] Mover set X listens to steering X.
    1. [ ] Steer. Hear sound.

1. [x] See [Horizontal Steering System](LudumDare42/Assets/UnityToykit/HorizontalSteeringSystem.cs)
    1. [ ] Tilt camera when steering.
    1. [ ] See horizon.

1. [x] Click System View (Unity Toykit)
    1. [x] Tap edge of screen to steer.
1. [x] Key System View (Unity Toykit)
    1. [x] Keyboard to steer.

## Spawn

### Editor flow

1. [ ] Lap Spawner System
    1. [ ] Lap Spawns
        1. [ ] Prefab
        1. [ ] Density
        1. [ ] Max Clones
    1. [ ] Lap Index
        1. [ ] Objects above lap index are not spawned.
    1. [ ] Wrap Width
        1. [ ] Objects wrap horizontally.
    1. [ ] Lap Depth
        1. [ ] Loops objects and increments lap index.
    1. [ ] Clone Depth
        1. [ ] Pre calculates Max Clones per spawn.
        1. [ ] Pre spawns.
    1. [ ] Set Position
        1. [ ] Objects furthest in of spawn type are selected to show next in grid.
        1. [ ] On Spawn.
        1. [ ] On Lap.
    1. [ ] Grid from depth and width.
        - Takes up a lot of space, yet fast to query.  Example: <https://github.com/jakesgordon/javascript-racer>
        1. [ ] Cell
            1. [ ] Spawned
            1. [ ] Lap Index

1. [ ] Lap Spawn Move Listener
    1. [ ] Lap Spawn listens to Move System On Moved.
    1. [ ] Move System listens to On Spawn.

1. [ ] Lap Spawn Music Listener
    1. [ ] Sync music beats per minute with speed.
    1. [ ] Sync rhythm of music and metrics of grid cell size and speed.

## Collider

### Editor flow

1. [ ] Collider
    1. [x] See vehicle.
    1. [ ] Rigid Body
    1. [ ] On Collision

1. [ ] Game Over Collider Listener
    1. [ ] Move System Pause listens to Collider On Collision
    1. [ ] Game Over root Set Active listens to On Collision
    1. [ ] Sound plays.

1. [ ] Game over. Restart.
1. [ ] Game over. Fade out blocks. Reset.

## Score

1. [ ] Read score by distance.
1. [ ] Read top score.
1. [ ] Score. Most significant digit represents lap.
1. [ ] Score. Sum of blocks passed.  "MB"
