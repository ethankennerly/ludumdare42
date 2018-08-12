using FineGameDesign.Utils;
using UnityEngine;

namespace FineGameDesign.LudumDare42
{
    /// <summary>
    /// 1. [ ] Move System Pause listens to Collider On Collision
    /// 1. [ ] Game Over root Set Active listens to On Collision
    /// 1. [ ] Sound plays.
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    public sealed class ColliderGameOver : MonoBehaviour
    {
        [Header("Optional")]
        [SerializeField]
        private AudioSource m_AudioSource;

        [Header("Optional")]
        [SerializeField]
        private AudioClip m_TriggerClip;

        private void OnTriggerEnter(Collider other)
        {
            DebugUtil.Log("OnTriggerEnter: this=" + this + " other=" + other);
            TryPlaySound();
        }

        private void TryPlaySound()
        {
            if (m_AudioSource == null)
                return;

            if (m_TriggerClip == null)
                return;

            m_AudioSource.PlayOneShot(m_TriggerClip);
        }
    }
}
