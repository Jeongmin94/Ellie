using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerAim : MonoBehaviour
    {
        public Cinemachine.AxisState xAxis;
        public Cinemachine.AxisState yAxis;

        public Transform cameraLookAt;

        void Start()
        {

        }

        void Update()
        {
            xAxis.Update(Time.deltaTime);
            yAxis.Update(Time.deltaTime);

            cameraLookAt.eulerAngles = new Vector3(yAxis.Value, xAxis.Value, 0);
        }
    }
}