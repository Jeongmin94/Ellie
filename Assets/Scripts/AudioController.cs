using Assets.Scripts.Managers;
using System.Collections;
using UnityEngine;
using static Assets.Scripts.Managers.SoundManager;

namespace Assets.Scripts
{
    public class AudioController : Poolable
    {
        public AudioClip clip;
        public float volume;

        private AudioSource audioSource;

        public bool isPaused = false;
        private void OnEnable()
        {
            audioSource = GetComponent<AudioSource>();
        }
        public void SetClip(AudioClip clip)
        {
            audioSource.clip = clip;
        }

        public void SetVolume(float volume)
        {
            this.volume = volume;
        }
        public void Play(float pitch, SoundType type)
        {
            audioSource.pitch = pitch;
            if(type == SoundType.Bgm)
            {
                audioSource.loop = true;
            }
            audioSource.Play();
        }

        public void Stop()
        {
            audioSource.loop = false;
            audioSource.Stop();
            audioSource.pitch = 1.0f;
        }

        public void Pause()
        {
            audioSource.Pause();
            isPaused = true;
        }

        public void Resume()
        {
            if(isPaused)
            {
                audioSource.Play();
                isPaused = false;
            }
        }

        public void Activate3DEffect()
        {
            audioSource.spatialBlend = 0.5f;
            audioSource.minDistance = 2f;
            audioSource.maxDistance = 5f;
        }

        public void Deactivate3DEffect()
        {
            audioSource.spatialBlend = 0;
        }
    }
}