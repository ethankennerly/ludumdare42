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
    /// 1. [ ] Visible Depth Min
    /// 1. [ ] Visible Depth Max
    ///     1. [ ] Objects furthest in of spawn type are selected to show next in grid.
    /// 1. [ ] Move Position
    ///     1. [ ] On Spawn.
    ///     1. [ ] On Loop.
    /// 1. [ ] Grid from depth and width.
    ///     1. [ ] Map image x to world x.
    ///     1. [ ] Map image y to world -z.
    ///     1. [ ] Align bottom center of image to 0, 0, 0.
    ///     1. [ ] Map gray scale channel red [0..255] to number loop index.
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
        public event Action<Transform[]> onSpawned;

        [SerializeField]
        private Transform m_SpawnParent;

        private ByteArray2D m_Layout;
        public ByteArray2D layout
        {
            get { return m_Layout; }
            set { m_Layout = value; }
        }

        public void Move(Vector3 step)
        {
        }
    }
}
