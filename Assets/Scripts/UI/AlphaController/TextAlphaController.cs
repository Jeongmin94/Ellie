using System.Collections;
using TMPro;
using UnityEngine;

namespace UI.AlphaController
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
            var timeAcc = 0.0f;
            textMeshProUGUI.color = start;

            var wfef = new WaitForEndOfFrame();
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