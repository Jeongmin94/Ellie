using Channels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Channels.Camera
{
    public enum CameraShakingEffectType
    {
        Start,
        Stop,
    }
    public class CameraPayload : IBaseEventPayload
    {
        public CameraShakingEffectType type = CameraShakingEffectType.Stop;
        public float shakeIntensity;
        public float shakeTime;
    }

    public class CameraChannel : BaseEventChannel
    {
        public override void ReceiveMessage(IBaseEventPayload payload)
        {
            if (payload is not CameraPayload) return;
            Publish(payload);
        }
    }
}
