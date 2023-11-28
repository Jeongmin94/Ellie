using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    public class SoundManager : Singleton<SoundManager>
    {
        private enum SoundType
        {
            Bgm,
            Sfx,
            End
        }

        private Pool audioSourcePool;
        private GameObject audioSource = new();
        private void InitAudioSourcePool()
        {
            audioSource.AddComponent<AudioSource>();
            audioSourcePool = PoolManager.Instance.CreatePool(audioSource, 10);
        }
    }
}
