using System;
using UnityEngine;

namespace FineGameDesign.Utils
{
    /// <summary>
    /// 1. [ ] Loop Spawner Mover Bridge
    ///     1. [x] Loop Spawner Move Position listens to Mover System On Moved.
    ///     1. [ ] Mover System Add Object listens to Loop Spawner On Spawned.
    /// </summary>
    public sealed class LoopSpawnerMoverBridge : MonoBehaviour
    {
        private Action<Vector3> m_OnMoved;
        private Action<GameObject> m_OnSpawned;

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
        }

        private void RemoveListeners()
        {
            MoverSystem.instance.onMoved -= m_OnMoved;
        }
    }
}
