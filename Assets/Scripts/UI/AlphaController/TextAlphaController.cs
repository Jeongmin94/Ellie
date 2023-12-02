using System.Collections;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class TextAlphaController : AlphaController<TextMeshProUGUI>
    {
        private TextMeshProUGUI textMeshProUGUI;

        private void Awake()
        {
            textMeshProUGUI = gameObject.GetComponent<TextMeshProUGUI>();
        }

        public override IEnumerator ChangeAlpha(Color start, Color end, float duration)
        {
            float timeAcc = 0.0f;
            textMeshProUGUI.color = start;

            WaitForEndOfFrame wfef = new WaitForEndOfFrame();
            while (timeAcc <= duration)
            {
                textMeshProUGUI.color = Color.Lerp(start, end, timeAcc / duration);

                yield return wfef;
                timeAcc += Time.deltaTime;
            }

            textMeshProUGUI.color = end;
        }
    }
}