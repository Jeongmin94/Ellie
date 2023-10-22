using System.Collections;
using Assets.Scripts.UI.Framework.Images;

namespace Assets.Scripts.UI.Player
{
    public class UIPlayerHealthImage : UIBaseImage
    {
        private void Awake()
        {
            base.Init();
        }

        public IEnumerator ChangeImageFillAmount(FillAmountType type, int total, int idx, float time,
            bool reverse = false)
        {
            float target = ((reverse ? (float)(idx + 1) : (float)idx)) / (float)total;
            return ChangeImageFillAmount(type, target, time, reverse);
        }

        public void ChangeImageFillAmount(FillAmountType type, int total, int idx, bool reverse = false)
        {
            float target = ((reverse ? (float)(idx + 1) : (float)idx)) / (float)total;
            ChangeImageFillAmount(type, target, reverse);
        }
    }
}