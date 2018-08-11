# Engineering

## Features

1. [x] Tap button to start.
1. [x] See vehicle.
1. [x] Read instruction.
1. [x] See a block.
1. [x] Tilt camera to see maximum blocks.
1. [ ] Move blocks at configurable speed.
1. [ ] Shuffle blocks to place in a row at maximum density.
1. [ ] Collide with block: See game over.
1. [ ] Game over. Restart.
1. [ ] Game over. Fade out blocks. Reset.
1. [ ] See horizon.
1. [ ] Keyboard to steer.
1. [ ] Tap edge of screen to steer.
1. [ ] Read score by distance. Read top score.
1. [ ] Pool cubes.
1. [ ] Wrap horizontal space of blocks.
1. [ ] Wrap depth of space of blocks.
1. [ ] Shuffle blocks to place in a row at maximum density.
1. [ ] Sync music beats per minute with speed.
1. [ ] Before next lap enters camera, place next layer of blocks.
1. [ ] As start next lap, fade in next layer of music.
1. [ ] Collide. Hear sound.
1. [ ] Steer. Hear sound.
1. [ ] Sync rhythm of music and metrics of grid cell size and speed.
1. [ ] Tilt camera when steering.
1. [ ] Pause.
1. [ ] Score. Most significant digit represents lap.
1. [ ] Score. Sum of blocks passed.  "MB"
1. [ ] Button to switch to tilt to steer.

# Architecture

## Motion

### Editor flow

1. [ ] Move System
    1. [ ] Is Paused.
    1. [ ] Speed (vector).
    1. [ ] Moving Objects.
    1. [ ] On Position Changed.
        1. [ ] Camera and vehicle stay still.
        1. [ ] Objects in grid slide in direction of camera along z axis.
        - Otherwise, floating point numbers becomes less precise.
        <http://davenewson.com/posts/2013/unity-coordinates-and-scales.html>
    1. [ ] Set X

1. [ ] Speed Pause Button.
    1. [ ] Pause Else Resume.

1. [ ] Horizontal Steer System
    1. [ ] X Axis Dead Zone.
        1. [ ] Listens to Key System On Key Down X Y.
        1. [ ] Listens to Click System On Axis X Y.
        1. [ ] If X in Dead Zone, stop here.
        1. [ ] If reset X time remaining, stop here.
        1. [ ] Publish On Steer X.
    1. [ ] Reset X Time
        1. [ ] Afterward reset X to zero.

1. [x] Click System View (Unity Toykit)
1. [ ] Key System View (Unity Toykit)

## Spawn

### Editor flow

1. [ ] Lap Spawn System
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
    1. [ ] Lap Spawn listens to Move System On Position Changed.
    1. [ ] Move System listens to On Spawn.

## Collider

### Editor flow

1. [ ] Collider
    1. [ ] Rigid Body
    1. [ ] On Collision

1. [ ] Game Over Collider Listener
    1. [ ] Move System Pause listens to Collider On Collision
    1. [ ] Game Over root Set Active listens to On Collision
    1. [ ] Sound plays.
