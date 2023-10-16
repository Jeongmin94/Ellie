using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerAim : MonoBehaviour
    {
        public Cinemachine.AxisState xAxis;
        public Cinemachine.AxisState yAxis;

        public Transform cameraLookAt;

        private float zoomingCoeff;
        void Start()
        {

        }

        void Update()
        {
            xAxis.Update(Time.deltaTime / Time.timeScale);
            yAxis.Update(Time.deltaTime / Time.timeScale);

            cameraLookAt.eulerAngles = new Vector3(yAxis.Value, xAxis.Value, 0);
        }
    }
}