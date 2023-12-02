using Assets.Scripts.Managers;
using UnityEngine;

namespace Data.UI.Config
{
    [CreateAssetMenu(fileName = "EffectVolumeData", menuName = "UI/Config/Settings/EffectVolume", order = 0)]
    public class EffectVolumeData : IntegerOptionData
    {
        public override void OnIndexChanged(int value)
        {
            base.OnIndexChanged(value);

            float ratio = (float)currentIdx / (values.Count - 1);
            SoundManager.Instance.SetVolume(SoundManager.SoundType.Sfx, ratio);
        }
    }
}