using Assets.Scripts.Managers;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerAim : MonoBehaviour
    {
        public Cinemachine.AxisState xAxis;
        public Cinemachine.AxisState yAxis;

        public Transform cameraLookAt;

        public bool canAim = true;

        private float zoomingCoeff;

        void Update()
        {
            if (!InputManager.Instance.CanInput)
                return;
            if (!canAim) return;
            
            float ts = Time.timeScale == 0f ? 1f : Time.timeScale;

            xAxis.Update(Time.deltaTime / ts);
            yAxis.Update(Time.deltaTime / ts);

            cameraLookAt.eulerAngles = new Vector3(yAxis.Value, xAxis.Value, 0);
        }
    }
}