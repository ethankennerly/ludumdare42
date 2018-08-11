using System;
using UnityEngine;

namespace FineGameDesign.Utils
{
    /// <summary>
    /// 1. [ ] Mover System
    ///     1. [ ] Is Paused.
    ///     1. [ ] Speed (vector).
    ///     1. [ ] Moving Objects.
    ///     1. [ ] Publishes On Position Changed.
    ///         1. [ ] Camera and vehicle stay still.
    ///         1. [ ] Objects in grid slide in direction of camera along z axis.
    ///         - Otherwise, floating point numbers becomes less precise.
    ///         <http://davenewson.com/posts/2013/unity-coordinates-and-scales.html>
    ///     1. [ ] Set X Speed
    /// </summary>
    [Serializable]
    public sealed class MoverSystem : ASingleton<MoverSystem>
    {
        [SerializeField]
        private Vector3 m_Speed;

        [SerializeField]
        private Transform[] m_Transforms;

        public void Update(float deltaTime)
        {
            Vector3 step = deltaTime * m_Speed;
            foreach (Transform transform in m_Transforms)
            {
                transform.position += step;
            }
        }
    }
}
