using Assets.Scripts.Data.UI.Dialog;
using TMPro;
using UnityEngine;

namespace Data.UI.Opening
{
    [CreateAssetMenu(fileName = "OpeningTextData", menuName = "UI/Opening/OpeningTextData")]
    public class OpeningTextData : DialogTypographyData
    {
        [SerializeField] public TextAlignmentOptions alignmentOptions;
        [SerializeField] public bool enableAutoSizing;
    }
}