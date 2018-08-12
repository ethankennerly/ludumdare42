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
    1. [x] Configure camera rotation to see blocks.
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

## Level Reader

1. [ ] Map image x to world x.
1. [ ] Map image y to world -z.
1. [ ] Map red channel to integer.
1. [ ] Align bottom center of image to 0, 0, 0.

## Spawn

### Editor flow

1. [ ] See [Loop Spawner System](LudumDare/Assets/Scripts/LoopSpawnerSystem.cs)

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
