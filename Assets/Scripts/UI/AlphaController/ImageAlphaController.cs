using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class ImageAlphaController : AlphaController<Image>
    {
        private Image image;

        private void Awake()
        {
            image = gameObject.GetComponent<Image>();
        }

        public override IEnumerator ChangeAlpha(Color start, Color end, float duration)
        {
            float timeAcc = 0.0f;
            image.color = start;

            WaitForEndOfFrame wfef = new WaitForEndOfFrame();
            while (timeAcc <= duration)
            {
                image.color = Color.Lerp(start, end, timeAcc / duration);

                yield return wfef;
                timeAcc += Time.deltaTime;
            }

            image.color = end;
        }
    }
}