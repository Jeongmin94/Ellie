using Assets.Scripts.Data.UI.Dialog;
using TMPro;
using UnityEngine;

namespace Data.UI.Opening
{
    [CreateAssetMenu(fileName = "TextData", menuName = "UI/TextData")]
    public class TextTypographyData : DialogTypographyData
    {
        [SerializeField] public TextAlignmentOptions alignmentOptions;
        [SerializeField] public bool enableAutoSizing;
        [SerializeField] public string title;

        [Header("마우스 호버 시 색")] [SerializeField]
        public Color highlightedColor = new(245, 245, 245, 255);

        [Header("마우스가 버튼을 누르고 있을 때의 색")] [SerializeField]
        public Color pressedColor = new(200, 200, 200, 255);

        [Header("버튼을 클릭했을 때의 색")] [SerializeField]
        public Color selectedColor = new(245, 245, 245, 255);

        [Header("버튼 클릭을 해제했을 때의 색")] [SerializeField]
        public Color disabledColor = new(200, 200, 200, 128);

        public static void SetTextTypographyData(TextMeshProUGUI tmp, TextTypographyData data)
        {
            SetDialogTypography(tmp, data);
            tmp.alignment = data.alignmentOptions;
            tmp.enableAutoSizing = data.enableAutoSizing;
            tmp.text = data.title;
        }
    }
}