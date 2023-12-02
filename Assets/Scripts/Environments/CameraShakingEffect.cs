using Cinemachine;
using System.Collections;
using System.Threading;
using UnityEngine;

namespace Assets.Scripts.Environments
{
    public class CameraShakingEffect : MonoBehaviour
    {
        private CinemachineVirtualCamera virtualCamera;
        private CinemachineBasicMultiChannelPerlin perlin;


        private float curintensity;
        private float timer;
        private void Awake()
        {
            virtualCamera = GetComponent<CinemachineVirtualCamera>();
            perlin = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        }

        private void Start()
        {
            StopShakeCamera();
        }

        public void StopShakeCamera()
        {
            perlin.m_AmplitudeGain = 0;
            timer = 0;
        }
        public void ShakeCamera(float shakeIntensity, float shakeTime)
        {
            //if (!gameObject.activeSelf) return;
            timer = shakeTime;
            perlin.m_AmplitudeGain = shakeIntensity;
        }

        // Update is called once per frame
        void LateUpdate()
        {
            //for test
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                ShakeCamera(1f, 0.2f);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                ShakeCamera(2f, 0.3f);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                ShakeCamera(3f, 0.4f);
            }

            if (timer > 0)
            {
                timer -= Time.deltaTime;
                if (timer < 0)
                    StopShakeCamera();
            }
        }
    }
}