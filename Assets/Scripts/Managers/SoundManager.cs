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
        //private IDictionary<>
        private AudioSource[] audioSources = new AudioSource[(int)SoundType.End];
        //Dictionary<string, AudioCilp> au
    }
}
