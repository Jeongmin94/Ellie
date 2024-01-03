using Channels.Components;
using Channels.Type;
using UnityEngine;

namespace Channels.Camera
{
    public enum CameraShakingEffectType
    {
        Start,
        Stop
    }

    public class CameraPayload : IBaseEventPayload
    {
        public float shakeIntensity;
        public float shakeTime;
        public CameraShakingEffectType type = CameraShakingEffectType.Stop;
    }

    public class CameraChannel : BaseEventChannel
    {
        public static void ShakeCamera(TicketMachine ticketMachine, float shakeIntensity, float shakeTime = 0.1f)
        {
            ticketMachine.SendMessage(ChannelType.Camera, new CameraPayload
            {
                type = CameraShakingEffectType.Start,
                shakeIntensity = shakeIntensity,
                shakeTime = shakeTime,
            });
        }
        
        public override void ReceiveMessage(IBaseEventPayload payload)
        {
            if (payload is not CameraPayload)
            {
                return;
            }
            
            Publish(payload);
        }
    }
}
