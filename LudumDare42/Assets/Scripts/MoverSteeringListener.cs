using System;
using UnityEngine;

namespace FineGameDesign.Utils
{
    public sealed class MoverSteeringListener : MonoBehaviour
    {
        private Action<float> m_OnSteerX;

        private void OnEnable()
        {
            AddListener();
        }

        private void OnDisable()
        {
            RemoveListener();
        }

        private void AddListener()
        {
            if (m_OnSteerX == null)
                m_OnSteerX = MoverSystem.instance.SetSpeedX;

            HorizontalSteeringSystem.onSteerX -= m_OnSteerX;
            HorizontalSteeringSystem.onSteerX += m_OnSteerX;
        }

        private void RemoveListener()
        {
            HorizontalSteeringSystem.onSteerX -= m_OnSteerX;
        }
    }
}
