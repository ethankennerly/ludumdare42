using System;
using UnityEngine;

namespace FineGameDesign.Utils
{
    /// <summary>
    /// 1. [x] Mover System
    ///     1. [x] Speed (vector).
    ///     1. [x] Moving Objects.
    ///     1. [x] Publishes On Moved.
    ///         1. [x] Camera and vehicle stay still.
    ///         1. [x] Objects in grid slide in direction of camera along z axis.
    ///         - Otherwise, floating point numbers becomes less precise.
    ///         <http://davenewson.com/posts/2013/unity-coordinates-and-scales.html>
    ///     1. [x] Set Speed X
    /// </summary>
    [Serializable]
    public sealed class MoverSystem : ASingleton<MoverSystem>
    {
        public event Action<Vector3> onMoved;
        public event Action<int> onDepth;

        [SerializeField]
        private Vector3 m_Speed;

        [SerializeField]
        private Transform[] m_Transforms;

        private int m_DepthFloored = 0;
        private float m_Depth = 0;

        public void AddTransforms(Transform[] transforms)
        {
            int numNewTransforms = transforms.Length;
            int numTransforms = m_Transforms.Length;
            int totalTransforms = numTransforms + numNewTransforms;
            Array.Resize(ref m_Transforms, totalTransforms);
            Array.Copy(transforms, 0, m_Transforms, numTransforms, numNewTransforms);
        }

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

            SetDepth(m_Depth - step.z);
        }

        public void Initialize()
        {
            foreach (Transform transform in m_Transforms)
            {
                transform.position = Vector3.zero;
            }

            SetDepth(0f);
        }

        public void SetSpeedX(float x)
        {
            m_Speed.x = x;
        }

        private void SetDepth(float depth)
        {
            m_Depth = depth;
            int nextDepth = (int)m_Depth;
            if (nextDepth == m_DepthFloored)
                return;
            m_DepthFloored = nextDepth;
            if (onDepth != null)
            {
                onDepth(nextDepth);
            }
        }
    }
}
