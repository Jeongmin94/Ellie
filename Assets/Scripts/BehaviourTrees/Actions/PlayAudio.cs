using System;
using Data.Monster;
using TheKiwiCoder;

[Serializable]
public class PlayAudio : ActionNode
{
    public NodeProperty<MonsterAudioType> audioType;
    public NodeProperty<bool> isInteruptable;
    public NodeProperty<bool> isLoop;

    protected override void OnStart()
    {
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        if (!isInteruptable.Value)
        {
            if (context.audioSource.isPlaying)
            {
                return State.Success;
            }
        }

        if (isLoop.Value)
        {
            context.audioSource.loop = true;
        }
        else
        {
            context.audioSource.loop = false;
        }

        var clip = context.audioController.GetAudio(audioType.Value);
        if (clip != null)
        {
            context.audioSource.clip = clip;
            context.audioSource.Play();

            return State.Success;
        }

        return State.Failure;
    }
}