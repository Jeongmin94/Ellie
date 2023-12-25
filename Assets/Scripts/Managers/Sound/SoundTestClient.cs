using Sirenix.OdinInspector;
using UnityEngine;

namespace Managers.Sound
{
    public class SoundTestClient : MonoBehaviour
    {
        private void Update()
        {
        }

        [Button("BGM Start", ButtonSizes.Large)]
        private void StartBgm()
        {
            SoundManager.Instance.PlaySound(SoundManager.SoundType.Bgm, "BgmTest");
        }

        [Button("BGM Stop", ButtonSizes.Large)]
        private void StopBgm()
        {
            SoundManager.Instance.StopBgm();
        }

        [Button("BGM Pause", ButtonSizes.Large)]
        private void PauseBgm()
        {
            SoundManager.Instance.PauseBgm();
        }

        [Button("BGM Resume", ButtonSizes.Large)]
        private void ResumeBgm()
        {
            SoundManager.Instance.ResumeBgm();
        }

        [Button("SFX Play", ButtonSizes.Large)]
        private void PlaySfxtest()
        {
            SoundManager.Instance.PlaySound(SoundManager.SoundType.Sfx, "ellie_sound1", new Vector3(0, 3, 0));
        }

        [Button("UISFX Play", ButtonSizes.Large)]
        private void UISfxTest()
        {
            SoundManager.Instance.PlaySound(SoundManager.SoundType.UISfx, "ellie_sound1");
        }
    }
}