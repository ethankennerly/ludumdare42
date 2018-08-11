using System;
using UnityEngine;

namespace FineGameDesign.Utils
{
    /// <summary>
    /// 1. [ ] Loop Spawns
    ///     1. [ ] Prefab
    ///     1. [ ] Density
    ///     1. [ ] Max Clones
    /// 1. [ ] Loop Index
    ///     1. [ ] Objects above loop index are not spawned.
    /// 1. [ ] Wrap Width
    ///     1. [ ] Objects wrap horizontally.
    /// 1. [ ] Loop Depth
    ///     1. [ ] Loops objects and increments loop index.
    /// 1. [ ] Clone Depth
    ///     1. [ ] Pre calculates Max Clones per spawn.
    ///     1. [ ] Pre spawns.
    /// 1. [ ] Move Position
    ///     1. [ ] Objects furthest in of spawn type are selected to show next in grid.
    ///     1. [ ] On Spawn.
    ///     1. [ ] On Loop.
    /// 1. [ ] Grid from depth and width.
    ///     - Takes up a lot of space, yet fast to query.  Example: <a href="https://github.com/jakesgordon/javascript-racer"/>
    ///     1. [ ] Cell
    ///         1. [ ] Spawned
    ///         1. [ ] Loop Index
    ///
    /// 1. [ ] Loop Spawner Music Listener
    ///     1. [ ] Sync music beats per minute with speed.
    ///     1. [ ] Sync rhythm of music and metrics of grid cell size and speed.
    /// </summary>
    [Serializable]
    public sealed class LoopSpawnerSystem : ASingleton<LoopSpawnerSystem>
    {
        public event Action<GameObject> onSpawned;

        public void Move(Vector3 step)
        {
        }
    }
}
