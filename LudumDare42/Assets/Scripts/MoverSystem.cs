using System;
using UnityEngine;

namespace FineGameDesign.Utils
{
    /// <summary>
    /// 1. [ ] Mover System
    ///     1. [x] Speed (vector).
    ///     1. [x] Moving Objects.
    ///     1. [ ] Publishes On Position Changed.
    ///         1. [x] Camera and vehicle stay still.
    ///         1. [x] Objects in grid slide in direction of camera along z axis.
    ///         - Otherwise, floating point numbers becomes less precise.
    ///         <http://davenewson.com/posts/2013/unity-coordinates-and-scales.html>
    ///     1. [ ] Set X Speed
    /// </summary>
    [Serializable]
    public sealed class MoverSystem : ASingleton<MoverSystem>
    {
        public static event Action<Vector3> onMoved;

        [SerializeField]
        private Vector3 m_Speed;

        [SerializeField]
        private Transform[] m_Transforms;

        private Vector3 m_Position;

        public void Update(float deltaTime)
        {
            Vector3 step = deltaTime * m_Speed;
            if (step == Vector3.zero)
                return;

            foreach (Transform transform in m_Transforms)
            {
                transform.position += step;
            }

            if (onMoved != null)
            {
                onMoved(step);
            }
        }
    }
}
