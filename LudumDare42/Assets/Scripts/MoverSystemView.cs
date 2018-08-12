using System;
using TMPro;
using UnityEngine;

namespace FineGameDesign.Utils
{
    /// <summary>
    /// Only updates when not paused.
    /// </summary>
    public sealed class MoverSystemView : ASingletonView<MoverSystem>
    {
        [Header("Optional")]
        [SerializeField]
        private TMP_Text m_DepthText;

        private Action<float> m_OnDeltaTime;

        private Action<int> m_OnDepth;

        private void OnEnable()
        {
            AddListener();

            controller.Initialize();
        }

        private void OnDisable()
        {
            RemoveListener();
        }

        private void AddListener()
        {
            if (m_OnDeltaTime == null)
                m_OnDeltaTime = controller.Update;

            PauseSystem.onDeltaTime -= m_OnDeltaTime;
            PauseSystem.onDeltaTime += m_OnDeltaTime;

            if (m_OnDepth == null)
                m_OnDepth = TrySetDepthText;
            controller.onDepth -= m_OnDepth;
            controller.onDepth += m_OnDepth;
        }

        private void RemoveListener()
        {
            PauseSystem.onDeltaTime -= m_OnDeltaTime;
            controller.onDepth -= m_OnDepth;
        }

        private void TrySetDepthText(int depth)
        {
            if (m_DepthText == null)
                return;

            m_DepthText.text = depth.ToString();
        }
    }
}
