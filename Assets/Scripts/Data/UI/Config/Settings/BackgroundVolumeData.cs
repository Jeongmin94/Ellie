using Assets.Scripts.Managers;
using UnityEngine;

namespace Data.UI.Config
{
    [CreateAssetMenu(fileName = "BackgroundVolumeData", menuName = "UI/Config/Settings/BackgroundVolumeData",
        order = 1)]
    public class BackgroundVolumeData : IntegerOptionData
    {
        public override void OnIndexChanged(int value)
        {
            var idx = currentIdx + value;
            if (idx < 0)
            {
                idx = 0;
            }

            if (idx >= values.Count)
            {
                idx = values.Count - 1;
            }

            currentIdx = idx;
            valueChangeAction?.Invoke(ValueString(values[currentIdx]));

            var ratio = (float)currentIdx / (values.Count - 1);
            SoundManager.Instance.SetVolume(SoundManager.SoundType.Bgm, ratio);
        }
    }
}