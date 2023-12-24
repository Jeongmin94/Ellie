using Assets.Scripts.Managers;
using Cinemachine;
using UnityEngine;

namespace Player
{
    public class PlayerAim : MonoBehaviour
    {
        public AxisState xAxis;
        public AxisState yAxis;

        public Transform cameraLookAt;

        public bool canAim = true;

        private float zoomingCoeff;

        private void Update()
        {
            if (!InputManager.Instance.CanInput)
            {
                return;
            }

            if (!canAim)
            {
                return;
            }

            var ts = Time.timeScale == 0f ? 1f : Time.timeScale;

            xAxis.Update(Time.deltaTime / ts);
            yAxis.Update(Time.deltaTime / ts);

            cameraLookAt.eulerAngles = new Vector3(yAxis.Value, xAxis.Value, 0);
        }
    }
}