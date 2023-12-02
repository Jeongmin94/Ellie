using Assets.Scripts.Managers;
using UnityEngine;

namespace Data.UI.Config
{
    [CreateAssetMenu(fileName = "BackgroundVolumeData", menuName = "UI/Config/Settings/BackgroundVolumeData", order = 1)]
    public class BackgroundVolumeData : IntegerOptionData
    {
        public override void OnIndexChanged(int value)
        {
            base.OnIndexChanged(value);

            float ratio = (float)currentIdx / (values.Count - 1);
            SoundManager.Instance.SetVolume(SoundManager.SoundType.Bgm, ratio);
        }
    }
}