using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Managers.Sound
{
    public class SoundManagerTestClient : MonoBehaviour
    {
        void Update()
        {

        }
        [Button("BGM Start", ButtonSizes.Large)]
        void StartBgm()
        {
            SoundManager.Instance.PlaySound(SoundManager.SoundType.Bgm, "BgmTest");
        }
        [Button("BGM Stop", ButtonSizes.Large)]

        void StopBgm()
        {
            SoundManager.Instance.StopBgm();

        }
        [Button("BGM Pause", ButtonSizes.Large)]

        void PauseBgm()
        {
            SoundManager.Instance.PauseBgm();
        }
        [Button("BGM Resume", ButtonSizes.Large)]

        void ResumeBgm()
        {
            SoundManager.Instance.ResumeBgm();
        }
        [Button("SFX Play", ButtonSizes.Large)]

        void PlaySfxtest()
        {
            SoundManager.Instance.PlaySound(SoundManager.SoundType.Sfx, "ellie_sound1", new Vector3(0,3,0));
        }
        [Button("UISFX Play", ButtonSizes.Large)]
        void UISfxTest()
        {
            SoundManager.Instance.PlaySound(SoundManager.SoundType.UISfx, "ellie_sound1");
        }


    }
}