# Engineering Features

- [ ] TODO
- [x] Done

## Motion

### Editor flow

1. [x] Pause.
    1. [x] Tap button to start.
    1. [x] Read instruction.
    1. [ ] Pause pauses music.
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

1. [x] See [Texture To Byte Array 2D](LudumDare42/Assets/UnityToykit/TextureToByteArray2D.cs)
1. [ ] See [Loop Spawner System](LudumDare/Assets/Scripts/LoopSpawnerSystem.cs)

## Spawn

1. [ ] See [Loop Spawner System](LudumDare/Assets/Scripts/LoopSpawnerSystem.cs)

## Collider

### Editor flow

1. [x] See [ColliderGameOver](LudumDare/Assets/Scripts/ColliderGameOver.cs)

1. [ ] Game over. Fade out blocks. Reset.

## Score

1. [ ] Read score by distance.
1. [ ] Read top score.
1. [ ] Score. Most significant digit represents lap.
1. [ ] Score. Sum of blocks passed.  "MB"

## Music sync

        >>> 32 * 4 / 137.
        0.9343065693430657
        >>> 32 * 4 / 137. * 60
        56.05839416058394
        >>> 32 * 4 / 137. * 60 * 11.416667
        640.0000186861314
