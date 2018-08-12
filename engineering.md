# Engineering Features

- [ ] TODO
- [x] Done

## Motion

### Editor flow

1. [x] Pause.
    1. [x] Tap button to start.
    1. [x] Read instruction.
    1. [x] Pause pauses music.
1. [x] See [Mover System](LudumDare42/Assets/Scripts/MoverSystem.cs)
    1. [x] See a block.
    1. [x] Move blocks at configurable speed.
    1. [x] Configure camera rotation to see blocks.
1. [x] Mover Steering Listener
    1. [x] Mover set X listens to steering X.
    1. [x] Steer. Hear sound.

1. [x] See [Horizontal Steering System](LudumDare42/Assets/UnityToykit/HorizontalSteeringSystem.cs)

1. [x] Click System View (Unity Toykit)
    1. [x] Tap edge of screen to steer.
1. [x] Key System View (Unity Toykit)
    1. [x] Keyboard to steer.

## Level Reader

1. [x] See [Texture To Byte Array 2D](LudumDare42/Assets/UnityToykit/TextureToByteArray2D.cs)
1. [x] See [Loop Spawner System](LudumDare/Assets/Scripts/LoopSpawnerSystem.cs)

## Spawn

1. [x] See [Loop Spawner System](LudumDare/Assets/Scripts/LoopSpawnerSystem.cs)

## Collider

### Editor flow

1. [x] See [ColliderGameOver](LudumDare/Assets/Scripts/ColliderGameOver.cs)

1. [x] Game over. Fade out blocks. Reset.

## Score

1. [x] Read score by distance.

## Music sync

- 137 beats per minute
- 32 bars per loop
- 4 beats per bar
- 5 blocks per beat
- 640 blocks per loop
- 11.4167 blocks per second

        >>> 32 * 4 / 137.
        0.9343065693430657
        >>> 32 * 4 / 137. * 60
        56.05839416058394
        >>> 32 * 4 / 137. * 60 * 11.416667
        640.0000186861314
