using System.Collections.Generic;
using UnityEngine;

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
    HeadShot
}


public class MonsterAudioController : MonoBehaviour
{
    [SerializeField] private MonsterAudioData data;
    private Dictionary<MonsterAudioType, AudioClip> audio;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audio = new Dictionary<MonsterAudioType, AudioClip>();
    }

    public AudioClip GetAudio(MonsterAudioType type)
    {
        AudioClip clip;
        if (audio.TryGetValue(type, out clip))
        {
            return clip;
        }

        audio.Add(type, data.GetAudioClip(type));
        if (audio.TryGetValue(type, out clip))
        {
            return clip;
        }

        return null;
    }

    public void PlayAudio(MonsterAudioType type)
    {
        var clip = GetAudio(type);
        audioSource.clip = clip;
        audioSource.Play();
    }
}