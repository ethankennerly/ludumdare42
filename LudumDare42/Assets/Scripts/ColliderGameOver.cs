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

        [Header("Optional")]
        [SerializeField]
        private GameObject m_TriggerActivatedObject;

        [Header("Optional")]
        [SerializeField]
        private Animator m_TriggerAnimator;

        [Header("Optional")]
        [SerializeField]
        private string m_TriggerAnimationName;

        private void OnTriggerEnter(Collider other)
        {
            PauseSystem.instance.Pause();
            PauseSystem.instance.state = PauseSystem.State.None;

            SceneNodeView.TrySetActive(m_TriggerActivatedObject, true);

            TryPlayAnimation(m_TriggerAnimator, m_TriggerAnimationName);

            TryPlaySound();
        }

        private void TryPlayAnimation(Animator animator, string animationName)
        {
            if (animator == null)
                return;

            if (string.IsNullOrEmpty(animationName))
                return;

            animator.Play(animationName, -1, 0f);
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
