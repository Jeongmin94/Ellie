using UnityEngine;

namespace Managers.Range
{
    public class RangeVisibility : MonoBehaviour
    {
        public Camera mainCamera;
        private MeshRenderer objectRenderer;

        private void Start()
        {
            mainCamera = Camera.main;

            objectRenderer = GetComponent<MeshRenderer>();
        }

        private void Update()
        {
            if (mainCamera.transform.position.y > transform.position.y)
            {
                objectRenderer.enabled = true;
            }
            else
            {
                objectRenderer.enabled = false;
            }
        }
    }
}