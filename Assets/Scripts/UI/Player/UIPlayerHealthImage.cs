using System.Collections;
using UI.Framework.Images;

namespace UI.Player
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