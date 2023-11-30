using UnityEngine;
using System.Collections.Generic;

public enum MonsterAudioType
{
    Idle,
    IdleAttack1,
    IdleAttack2,
    Move1,
    Move2,
    Hit,
    Dead,
    MoveSkill,
    MeleeAttack,
    MeleeAttackHit, 
    WeaponAttack,
    WeaponAttackPerform,
    WeaponAttackHit,
    ProjectileAttack,
    ProjectileFire,
    ProjectileHit,
}


public class MonsterAudioController : MonoBehaviour
{

    [SerializeField] private MonsterAudioData data;
    private Dictionary<MonsterAudioType, AudioClip> audio;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audio = new();
    }

    public AudioClip GetAudio(MonsterAudioType type)
    {
        AudioClip clip;
        if(audio.TryGetValue(type, out clip))
        {
            return clip;
        }
        else
        {
            audio.Add(type, data.GetAudioClip(type));
            if (audio.TryGetValue(type, out clip))
            {
                return clip;
            }
        }

        return null;
    }

    public void PlayAudio(MonsterAudioType type)
    {
        AudioClip clip = GetAudio(type);
        audioSource.clip = clip;
        audioSource.Play();
    }

}

