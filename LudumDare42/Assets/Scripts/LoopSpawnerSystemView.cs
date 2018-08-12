using System;
using UnityEngine;

namespace FineGameDesign.Utils
{
    public sealed class LoopSpawnerSystemView : ASingletonView<LoopSpawnerSystem>
    {
        [SerializeField]
        private TextureToByteArray2D m_TextureController;

        [SerializeField]
        private GameObject[] m_ActivatedOnLoopIndex;

        private Action<int> m_OnLoopIndexChanged;

        private void OnEnable()
        {
            AddListener();

            controller.ResetPosition();
            controller.Spawn(m_TextureController.GetRedChannel());
        }

        private void OnDisable()
        {
            RemoveListener();
        }

        private void AddListener()
        {
            if (m_OnLoopIndexChanged == null)
                m_OnLoopIndexChanged = TryActivateLoopObject;

            controller.onLoopIndexChanged -= m_OnLoopIndexChanged;
            controller.onLoopIndexChanged += m_OnLoopIndexChanged;
        }

        private void RemoveListener()
        {
            controller.onLoopIndexChanged -= m_OnLoopIndexChanged;
        }

        private void TryActivateLoopObject(int loopIndex)
        {
            if (loopIndex >= m_ActivatedOnLoopIndex.Length)
                return;

            GameObject loopObject = m_ActivatedOnLoopIndex[loopIndex];
            SceneNodeView.TrySetActive(loopObject, true);
        }
    }
}
