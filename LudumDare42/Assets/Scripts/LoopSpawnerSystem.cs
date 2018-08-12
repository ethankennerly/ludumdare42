using System;
using System.Collections.Generic;
using UnityEngine;

namespace FineGameDesign.Utils
{
    /// <summary>
    /// 1. [ ] Loop Prefab
    ///     1. [ ] Prefab
    ///     1. [ ] Max Clones
    /// 1. [ ] Loop Index
    ///     1. [ ] Objects above loop index are not spawned.
    /// 1. [ ] Wrap Width
    ///     1. [ ] Objects wrap horizontally.
    /// 1. [ ] Loop Depth
    ///     1. [ ] Loops objects and increments loop index.
    /// 1. [ ] Spawn Depth Min
    /// 1. [ ] Spawn Depth Max
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
        private Vector3 m_Origin = Vector3.zero;

        [SerializeField]
        private LoopPool[] m_LoopPools;

        [SerializeField]
        private int m_SpawnDepthMin = -10;

        [SerializeField]
        private int m_SpawnDepthMax = 50;

        [NonSerialized]
        private Vector3 m_Position;

        [NonSerialized]
        private byte m_LoopIndex;

        [NonSerialized]
        private byte m_MaxLoops;

        private struct Cell
        {
            public byte loopIndex;
            public bool spawned;

            public Cell(byte loopIndex, bool spawned)
            {
                this.loopIndex = loopIndex;
                this.spawned = spawned;
            }
        }

        [NonSerialized]
        private Cell[] m_Cells;
        [NonSerialized]
        private int m_NumColumns;
        [NonSerialized]
        private int m_NumRows;

        [Serializable]
        private class LoopPool
        {
            public byte threshold;
            public GameObject prefab;
            [Header("Too low: despawn. Too high: waste memory.")]
            public int maxClones = 128;
            public Transform spawnParent;

            [NonSerialized]
            public List<GameObject> pool;
            [NonSerialized]
            public GameObject[] spawnedObjects;
            [NonSerialized]
            private int spawnedIndex;

            public void Initialize(Vector3 offScreen)
            {
                spawnedObjects = new GameObject[maxClones];
                for (int index = 0; index < maxClones; ++index)
                {
                    GameObject gameObject = UnityEngine.Object.Instantiate(
                        prefab, offScreen, Quaternion.identity, spawnParent);
                    spawnedObjects[index] = gameObject;
                }
                spawnedIndex = 0;
            }

            public GameObject GetObject()
            {
                spawnedIndex++;
                if (spawnedIndex >= maxClones)
                    spawnedIndex = 0;
                return spawnedObjects[spawnedIndex];
            }
        }

        public void Spawn(ByteArray2D layout)
        {
            m_MaxLoops = (byte)m_LoopPools.Length;
            Vector3 offScreen = new Vector3(0f, 0f, m_SpawnDepthMin);
            foreach (LoopPool pool in m_LoopPools)
                pool.Initialize(offScreen);
            SetCells(layout);
            UpdateSpawning();
        }

        private void SetCells(ByteArray2D layout)
        {
            m_NumColumns = layout.width;
            m_NumRows = layout.height;
            int numCells = layout.bytes.Length;
            m_Cells = new Cell[numCells];
            for (int index = 0; index < numCells; ++index)
            {
                byte loopIndex = ByteToLoopIndex(layout.bytes[index]);
                m_Cells[index] = new Cell(loopIndex, false);
            }
        }

        private byte ByteToLoopIndex(byte source)
        {
            const byte maxByte = 255;
            if (source == 0)
                return maxByte;

            return (byte)0;

            // TODO:

            if (source > 0)
            {
                bool breakHere = true;
            }
            byte scaled = (byte)((source * m_MaxLoops) / maxByte);
            if (scaled >= m_MaxLoops)
                return 0;

            byte inverted = (byte)(m_MaxLoops - scaled);
            return inverted;
        }

        // 1. [x] Map image x to world x.
        // 1. [x] Map image y to world -z.
        // 1. [x] Align bottom center of image to 0, 0, 0.
        private void UpdateSpawning()
        {
            int rowSpawnRange = m_SpawnDepthMax - m_SpawnDepthMin;
            int rowLimit = rowSpawnRange;
            if (rowLimit > m_NumRows)
                rowLimit = m_NumRows;
            int rowPosition = -(int)m_Position.z;
            for (int rowFromMin = 0; rowFromMin < rowLimit; ++rowFromMin)
            {
                int row = (rowPosition + rowFromMin + m_SpawnDepthMin) % m_NumRows;
                while (row < 0)
                    row += m_NumRows;
                for (int column = 0; column < m_NumColumns; ++column)
                {
                    int cellIndex = row * m_NumColumns + column;
                    Cell cell = m_Cells[cellIndex];
                    if (cell.spawned && rowFromMin >= rowSpawnRange)
                    {
                        cell.spawned = false;
                        continue;
                    }
                    if (cell.loopIndex > m_LoopIndex)
                        continue;

                    if (cell.spawned)
                        continue;

                    cell.spawned = true;
                    LoopPool pool = m_LoopPools[m_LoopIndex];
                    GameObject clone = pool.GetObject();
                    clone.transform.position = Place(column, row);
                }
            }
        }

        private Vector3 Place(int column, int row)
        {
            float x = m_Position.x + column - m_NumColumns / 2;
            float y = 0f;
            float z = row + m_Position.z;
            Vector3 position = new Vector3(x, y, z);
            return position;
        }

        public void Move(Vector3 step)
        {
            m_Position += step;
            UpdateSpawning();
        }
    }
}
