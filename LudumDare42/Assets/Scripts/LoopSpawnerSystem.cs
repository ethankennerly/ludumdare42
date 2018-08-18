using System;
using System.Collections.Generic;
using UnityEngine;

namespace FineGameDesign.Utils
{
    /// <summary>
    /// 1. [x] Loop Prefab
    ///     1. [x] Prefab
    ///     1. [x] Max Clones
    /// 1. [x] Loop Index
    ///     1. [x] Objects above loop index are not spawned.
    /// 1. [x] Wrap Width
    ///     1. [x] Objects wrap horizontally.
    /// 1. [x] Loop Depth
    ///     1. [x] Loops objects and increments loop index.
    /// 1. [x] Spawn Depth Min
    /// 1. [x] Spawn Depth Max
    ///     1. [x] Objects furthest in of spawn type are selected to show next in grid.
    /// 1. [x] Move Position
    ///     1. [x] On Spawn.
    ///     1. [x] On Loop.
    /// 1. [x] Grid from depth and width.
    ///     1. [x] Map image x to world x.
    ///     1. [x] Map image y to world -z.
    ///     1. [x] Align bottom center of image to 0, 0, 0.
    ///     1. [x] Map gray scale channel red [0..255] to number loop index.
    ///     - Takes up a lot of space, yet fast to query.  Example: <a href="https://github.com/jakesgordon/javascript-racer"/>
    ///     1. [x] Cell
    ///         1. [x] Spawned
    ///         1. [x] Loop Index
    /// </summary>
    [Serializable]
    public sealed class LoopSpawnerSystem : ASingleton<LoopSpawnerSystem>
    {
        public event Action<int> onLoopIndexChanged;

        [SerializeField]
        private Vector3 m_Origin = Vector3.zero;

        [SerializeField]
        private LoopPool[] m_LoopPools;

        [SerializeField]
        private int m_SpawnDepthMin = -10;

        [SerializeField]
        private int m_SpawnDepthMax = 50;

        [SerializeField]
        private int m_SpawnColumnDistanceMax = 25;

        [NonSerialized]
        private Vector3 m_Position;
        [NonSerialized]
        private Vector3 m_WrappedPosition;

        [NonSerialized]
        private byte m_PositionLoopIndex = 255;

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

            /// <summary>
            /// This could pool objects to survive scene change by setting objects to not destroy on load.
            /// The objects would need repositioning.
            /// </summary>
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

        public void ResetPosition()
        {
            m_Position = m_Origin;
            m_PositionLoopIndex = 255;
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
        private byte ByteToLoopIndex(byte source, byte[] sortedThresholds, bool invert = false)
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

            // Byte wraps around to 255 below 0.
            for (byte loopIndex = maxIndex; loopIndex <= maxIndex; --loopIndex)
            {
                byte threshold = sortedThresholds[loopIndex];
                if (source >= threshold)
                {
                    if (invert)
                        return (byte)(maxIndex - loopIndex);

                    return loopIndex;
                }
            }
            return maxByte;
        }

        /// <summary>
        /// Does not spawn if position at cell is not at that loop index.
        /// Otherwise, when approaching the loop,
        /// cells at the end of the next loop spawn in the middle of the camera.
        /// </summary>
        private void UpdateSpawning()
        {
            m_WrappedPosition = WrapSpawnPosition(m_Position);
            int rowPosition = -(int)m_Position.z;
            if (rowPosition < 0)
                rowPosition = 0;

            UpdateLoopIndexes(rowPosition);

            int rowSpawnRange = m_SpawnDepthMax - m_SpawnDepthMin;
            int rowLimit = rowSpawnRange;
            if (rowLimit > m_NumRows)
                rowLimit = m_NumRows;

            int columnPosition = -(int)m_Position.x;

            for (int rowFromMin = 0; rowFromMin < rowLimit; ++rowFromMin)
            {
                int rowPositionAtCell = rowPosition + rowFromMin + m_SpawnDepthMin;
                byte loopIndexAtCell = GetLoopIndex(rowPositionAtCell);
                int row = rowPositionAtCell % m_NumRows;
                while (row < 0)
                    row += m_NumRows;
                for (int column = 0; column < m_NumColumns; ++column)
                {
                    int cellIndex = row * m_NumColumns + column;
                    Cell cell = m_Cells[cellIndex];
                    if (cell.spawned)
                    {
                        if (rowFromMin >= rowSpawnRange)
                        {
                            cell.spawned = false;
                            continue;
                        }

                        int columnDistance = column - columnPosition;
                        if (columnDistance < 0)
                            columnDistance = -columnDistance;
                        if (columnDistance >= m_SpawnColumnDistanceMax)
                        {
                            cell.spawned = false;
                            continue;
                        }
                    }
                    if (cell.loopIndex > loopIndexAtCell)
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

        private void UpdateLoopIndexes(int rowPosition)
        {
            byte nextLoopIndex = GetLoopIndex(rowPosition);
            if (nextLoopIndex == m_PositionLoopIndex)
                return;

            m_PositionLoopIndex = nextLoopIndex;
            if (onLoopIndexChanged == null)
                return;

            onLoopIndexChanged(nextLoopIndex);
        }

        private byte GetLoopIndex(int rowPosition)
        {
            byte loopIndex = (byte)(rowPosition / m_NumRows);
            if (loopIndex >= m_MaxLoops)
                loopIndex = (byte)(m_MaxLoops - 1);
            return loopIndex;
        }

        private Vector3 WrapSpawnPosition(Vector3 source)
        {
            return new Vector3(
                source.x % m_NumColumns,
                source.y,
                source.z % m_NumRows
            );
        }

        /// <summary>
        /// 1. [x] Map image x to world x.
        /// 1. [x] Map image y to world -z.
        /// 1. [x] Align bottom center of image to 0, 0, 0.
        /// </summary>
        private Vector3 Place(int column, int row, Vector3 wrappedPosition)
        {
            float x = wrappedPosition.x + column - m_NumColumns / 2;
            while (x < -m_SpawnColumnDistanceMax)
                x += m_NumColumns;
            while (x > m_SpawnColumnDistanceMax)
                x -= m_NumColumns;
            float y = -wrappedPosition.y;
            float z = row + wrappedPosition.z;
            while (z > m_SpawnDepthMax)
                z -= m_NumRows;
            while (z < m_SpawnDepthMin)
                z += m_NumRows;
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
