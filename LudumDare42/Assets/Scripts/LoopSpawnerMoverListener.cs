using System;
using UnityEngine;

namespace FineGameDesign.Utils
{
    /// <summary>
    /// 1. [x] Loop Spawner Mover Bridge
    ///     1. [x] Loop Spawner Move Position listens to Mover System On Moved.
    ///     1. [x] Mover Add Transforms listens to Loop Spawner On Spawned.
    /// </summary>
    public sealed class LoopSpawnerMoverBridge : MonoBehaviour
    {
        private Action<Vector3> m_OnMoved;
        private Action<Transform[]> m_OnSpawned;

        private void OnEnable()
        {
            AddListeners();
        }

        private void OnDisable()
        {
            RemoveListeners();
        }

        private void AddListeners()
        {
            if (m_OnMoved == null)
                m_OnMoved = LoopSpawnerSystem.instance.Move;
            MoverSystem.instance.onMoved -= m_OnMoved;
            MoverSystem.instance.onMoved += m_OnMoved;

            if (m_OnSpawned == null)
                m_OnSpawned = MoverSystem.instance.AddTransforms;
            LoopSpawnerSystem.instance.onSpawned -= m_OnSpawned;
            LoopSpawnerSystem.instance.onSpawned += m_OnSpawned;
        }

        private void RemoveListeners()
        {
            MoverSystem.instance.onMoved -= m_OnMoved;
            LoopSpawnerSystem.instance.onSpawned -= m_OnSpawned;
        }
    }
}
