﻿using Assets.Scripts.Managers;
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
            xAxis.Update(Time.deltaTime / Time.timeScale);
            yAxis.Update(Time.deltaTime / Time.timeScale);

            cameraLookAt.eulerAngles = new Vector3(yAxis.Value, xAxis.Value, 0);
        }
    }
}