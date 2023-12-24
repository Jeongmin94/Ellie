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