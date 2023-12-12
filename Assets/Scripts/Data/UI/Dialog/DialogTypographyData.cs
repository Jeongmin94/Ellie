using TMPro;
using UnityEngine;

namespace Assets.Scripts.Data.UI.Dialog
{
    [CreateAssetMenu(fileName = "DialogData", menuName = "UI/Dialog/DialogData", order = 0)]
    public class DialogTypographyData : ScriptableObject
    {
        [SerializeField] public TMP_FontAsset fontAsset;
        [SerializeField] public Color color;

        [SerializeField] public float fontSize;
        
        [Header("Spacing Options")]
        [SerializeField] public float characterSpacing;
        [SerializeField] public float wordSpacing;
        [SerializeField] public float paragraphSpacing;
        [SerializeField] public float lineSpacing;

        [Header("Outline Options")]
        [SerializeField] public bool useOutline = false;
        [SerializeField] public Color outlineColor;
        [SerializeField] public float outlineThickness;

        public static void SetDialogTypography(TextMeshProUGUI tmp, DialogTypographyData data)
        {
            tmp.font = data.fontAsset;
            tmp.color = data.color;
            tmp.fontSize = data.fontSize;

            tmp.characterSpacing = data.characterSpacing;
            tmp.wordSpacing = data.wordSpacing;
            tmp.paragraphSpacing = data.paragraphSpacing;
            tmp.lineSpacing = data.lineSpacing;

            if (data.useOutline)
            {
                tmp.outlineColor = data.outlineColor;
                tmp.outlineWidth = data.outlineThickness;
            }
        }
    }
}