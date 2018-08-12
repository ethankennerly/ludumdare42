using UnityEngine;

namespace FineGameDesign.Utils
{
    public sealed class LoopSpawnerSystemView : ASingletonView<LoopSpawnerSystem>
    {
        [SerializeField]
        private TextureToByteArray2D m_TextureController;

        private void OnEnable()
        {
            controller.layout = m_TextureController.GetRedChannel();
        }
    }
}
