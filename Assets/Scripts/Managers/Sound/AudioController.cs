using Assets.Scripts.Managers;
using UnityEngine;
using static Assets.Scripts.Managers.SoundManager;

namespace Assets.Scripts
{
    public class AudioController : Poolable
    {
        public AudioClip clip;
        public float volume;

        private AudioSource audioSource;

        public bool isPlaying = false;
        public bool isPaused = false;

        public override void PoolableDestroy()
        {
            SoundManager.Instance.OnPoolableDestroy(this);
        }

        private void OnEnable()
        {
            audioSource = GetComponent<AudioSource>();
        }

        public void SetClip(AudioClip clip)
        {
            this.clip = clip;
            audioSource.clip = clip;
        }

        public void SetVolume(float volume)
        {
            audioSource.volume = volume;
        }

        public void Play(float pitch, SoundType type)
        {
            isPlaying = true;
            audioSource.pitch = pitch;
            if (type == SoundType.Bgm || type == SoundType.Ambient)
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
            isPlaying = false;
        }

        public void Pause()
        {
            audioSource.Pause();
            isPaused = true;
        }

        public void Resume()
        {
            if (isPaused)
            {
                audioSource.Play();
                isPaused = false;
            }
        }

        public void Activate3DEffect()
        {
            audioSource.spatialBlend = 1f;
        }

        public void Deactivate3DEffect()
        {
            audioSource.spatialBlend = 0;
        }
    }
}