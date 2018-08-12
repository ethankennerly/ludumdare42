using System;

namespace FineGameDesign.Utils
{
    /// <summary>
    /// Only updates when not paused.
    /// </summary>
    public sealed class MoverSystemView : ASingletonView<MoverSystem>
    {
        private Action<float> m_OnDeltaTime;

        private void OnEnable()
        {
            controller.Initialize();

            AddListener();
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
        }

        private void RemoveListener()
        {
            PauseSystem.onDeltaTime -= m_OnDeltaTime;
        }
    }
}
