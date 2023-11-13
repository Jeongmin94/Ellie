using TMPro;
using UnityEngine;

namespace Assets.Scripts.Data.UI
{
    [CreateAssetMenu(fileName = "DialogData", menuName = "UI/Dialog/DialogData", order = 0)]
    public class DialogTypographyData : ScriptableObject
    {
        [SerializeField] public TMP_FontAsset fontAsset;
        [SerializeField] public Color color;

        [SerializeField] public float fontSize;
        [SerializeField] public float lineSpacing;
    }
}