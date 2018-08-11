using UnityEngine;

namespace FineGameDesign.Utils
{
    public sealed class MoverSystemView : ASingletonView<MoverSystem>
    {
        private void Update()
        {
            controller.Update(PauseSystem.instance.deltaTime);
        }
    }
}
