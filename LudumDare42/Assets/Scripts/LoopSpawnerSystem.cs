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
        private Vector3 m_WrappedPosition;

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
        [NonSerialized]
        private byte[] m_Thresholds;

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
            m_Thresholds = SortThresholds(m_LoopPools);
            for (int index = 0; index < numCells; ++index)
            {
                byte loopIndex = ByteToLoopIndex(layout.bytes[index], m_Thresholds);
                m_Cells[index] = new Cell(loopIndex, false);
            }
        }

        private byte[] SortThresholds(LoopPool[] pools)
        {
            int numThresholds = pools.Length;
            byte[] thresholds = new byte[numThresholds];
            for (int index = 0; index < numThresholds; ++index)
            {
                thresholds[index] = pools[index].threshold;
            }
            Array.Sort(thresholds);
            return thresholds;
        }

        /// <param name="source">0: Never spawns by assigning max 255.
        /// Example sources:
        ///     >>> 0x66
        ///     102
        ///     >>> 0x99
        ///     153
        ///     >>> 0xff
        ///     255
        ///     >>> 0xcc
        ///     204
        /// </param>
        /// <param name="sortedThresholds">Positive thresholds. Less than 255 of them.</param>
        private byte ByteToLoopIndex(byte source, byte[] sortedThresholds)
        {
            const byte maxByte = 255;
            Debug.Assert(sortedThresholds.Length < maxByte && sortedThresholds.Length >= 1,
                "Too many thresholds. Expected between 1 and 254 thresholds. Got " +
                sortedThresholds.Length
            );
            byte maxIndex;
            if (sortedThresholds.Length >= maxByte)
                maxIndex = maxByte - 1;
            else
                maxIndex = (byte)(sortedThresholds.Length - 1);

            for (byte loopIndex = maxIndex; true; --loopIndex)
            {
                byte threshold = sortedThresholds[loopIndex];
                if (source >= threshold)
                    return loopIndex;
                if (loopIndex == 0)
                    break;
            }
            return maxByte;
        }

        // 1. [x] Map image x to world x.
        // 1. [x] Map image y to world -z.
        // 1. [x] Align bottom center of image to 0, 0, 0.
        private void UpdateSpawning()
        {
            m_WrappedPosition = WrapSpawnPosition(m_Position);
            int rowPosition = -(int)m_Position.z;
            if (rowPosition < 0)
                rowPosition = 0;
            int spawnDepth = rowPosition + m_SpawnDepthMax;
            m_LoopIndex = (byte)(spawnDepth / m_NumRows);
            if (m_LoopIndex >= m_MaxLoops)
                m_LoopIndex = (byte)(m_MaxLoops - 1);

            int rowSpawnRange = m_SpawnDepthMax - m_SpawnDepthMin;
            int rowLimit = rowSpawnRange;
            if (rowLimit > m_NumRows)
                rowLimit = m_NumRows;
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
                    LoopPool pool = m_LoopPools[cell.loopIndex];
                    GameObject clone = pool.GetObject();
                    clone.transform.position = Place(column, row, m_WrappedPosition);
                }
            }
        }

        private Vector3 WrapSpawnPosition(Vector3 source)
        {
            return new Vector3(
                source.x % m_NumColumns,
                source.y,
                source.z % m_NumRows
            );
        }

        private Vector3 Place(int column, int row, Vector3 wrappedPosition)
        {
            float x = m_WrappedPosition.x + column - m_NumColumns / 2;
            float y = -m_WrappedPosition.y;
            float z = row + m_WrappedPosition.z;
            if (z < m_SpawnDepthMin)
                z += m_NumRows;
            else if (z > m_SpawnDepthMax)
                z -= m_NumRows;
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
