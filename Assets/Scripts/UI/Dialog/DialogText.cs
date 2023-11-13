using System.Collections;
using System.Text;
using Assets.Scripts.Data.UI;
using Assets.Scripts.Data.UI.Dialog;
using Assets.Scripts.Utils;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI.Dialog
{
    public class DialogText : MonoBehaviour
    {
        private TextMeshProUGUI dialogText;

        public void InitDialogText()
        {
            dialogText = gameObject.FindChild<TextMeshProUGUI>(null, true);
        }

        public void InitTypography(DialogTypographyData typographyData)
        {
            dialogText.font = typographyData.fontAsset;
            dialogText.fontSize = typographyData.fontSize;
            dialogText.lineSpacing = typographyData.lineSpacing;
            dialogText.color = typographyData.color;
        }

        public void SetText(string text)
        {
            dialogText.text = text;
        }

        public void ClearText()
        {
            dialogText.text = string.Empty;
        }

        public void Play(string text, float interval)
        {
            StartCoroutine(ReadText(text, interval));
        }

        private IEnumerator ReadText(string text, float interval)
        {
            WaitForSeconds wfs = new WaitForSeconds(interval);
            StringBuilder sb = new StringBuilder();

            foreach (var ch in text)
            {
                sb.Append(ch);
                dialogText.text = sb.ToString();

                yield return wfs;
            }

            // !TODO: 텍스트 출력이 완료되면 넘기기 버튼 활성화가 가능해짐
        }
    }
}