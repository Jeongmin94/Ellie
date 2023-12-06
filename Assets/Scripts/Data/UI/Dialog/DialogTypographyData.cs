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
    }
}