using UnityEngine;

namespace Outline
{
    public class OutlineControllerTestClient : MonoBehaviour
    {
        public OutlineController outlineController;
        public MeshRenderer target;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                outlineController.AddOutline(target, OutlineType.InteractiveOutline);
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                outlineController.RemoveMaterial(target, OutlineType.InteractiveOutline);
            }
        }
    }
}