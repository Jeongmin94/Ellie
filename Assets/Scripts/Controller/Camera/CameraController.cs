using System.Collections;
using Channels.Camera;
using Channels.Components;
using Channels.Type;
using UnityEngine;
using Utils;

namespace Controller.Camera
{
    public class CameraController : BaseController
    {
        private Transform camera;
        
        private void Awake()
        {
            camera = UnityEngine.Camera.main.transform;
        }

        public override void InitController()
        {
            InitTicketMachine();
        }
        
        private void InitTicketMachine()
        {
            var ticketMachine = gameObject.GetOrAddComponent<TicketMachine>();
            ticketMachine.AddTickets(ChannelType.Camera);
            // ticketMachine.RegisterObserver(ChannelType.Camera, OnNotifyCamera);
        }

        private void OnNotifyCamera(IBaseEventPayload payload)
        {
            if (payload is not CameraPayload cameraPalyaod)
            {
                return;
            }
            
            StartCoroutine(ShakeCoroutine
                (cameraPalyaod.shakeIntensity, cameraPalyaod.shakeTime));
        }
        
        private IEnumerator ShakeCoroutine(float shakeIntensity, float shakeDuration)
        {
            var elapsed = 0.0f;

            var originalPosition = camera.position;

            while (elapsed < shakeDuration)
            {
                camera.position = originalPosition + Random.insideUnitSphere * shakeIntensity;
                elapsed += Time.deltaTime;
                yield return null;
            }

            camera.position = originalPosition;
        }
    }
}
