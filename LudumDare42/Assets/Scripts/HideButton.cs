using UnityEngine;
using UnityEngine.UI;

namespace FineGameDesign.Utils
{
    public sealed class HideButton : MonoBehaviour
    {
        [SerializeField]
        private Button m_HideButton;

        [SerializeField]
        private GameObject m_HideObject;

        private void OnEnable()
        {
            m_HideButton.onClick.AddListener(HideObject);
        }

        private void OnDisable()
        {
            m_HideButton.onClick.RemoveListener(HideObject);
        }

        private void HideObject()
        {
            m_HideObject.SetActive(false);
        }
    }
}
