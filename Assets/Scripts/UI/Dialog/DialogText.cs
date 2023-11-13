using Assets.Scripts.Data.UI;
using Assets.Scripts.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;

namespace Assets.Scripts.UI
{
    // !TODO: 대화창 텍스트 연출 방식
    public class DialogText : MonoBehaviour
    {
        private TextMeshProUGUI dialogText;

        public void InitDialogText()
        {
            dialogText = gameObject.FindChild<TextMeshProUGUI>();
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
    }
}