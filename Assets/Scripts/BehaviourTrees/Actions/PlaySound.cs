using TheKiwiCoder;
using Assets.Scripts.Managers;

[System.Serializable]
public class PlaySound : ActionNode
{
    public NodeProperty<string> soundName;
    public NodeProperty<bool> isUISfx;

    protected override void OnStart() {
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        if(isUISfx.Value)
        {
            SoundManager.Instance.PlaySound(SoundManager.SoundType.UISfx, soundName.Value, context.transform.position);
        }
        else
        {
            SoundManager.Instance.PlaySound(SoundManager.SoundType.Sfx, soundName.Value, context.transform.position);
        }

        return State.Success;
    }
}
