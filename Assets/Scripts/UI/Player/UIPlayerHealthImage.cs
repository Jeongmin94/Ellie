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
            var target = (reverse ? idx + 1 : (float)idx) / total;
            return ChangeImageFillAmount(type, target, time);
        }

        public void ChangeImageFillAmount(FillAmountType type, int total, int idx, bool reverse = false)
        {
            var target = (reverse ? idx + 1 : (float)idx) / total;
            ChangeImageFillAmount(type, target);
        }
    }
}