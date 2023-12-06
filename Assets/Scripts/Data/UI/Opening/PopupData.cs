using Assets.Scripts.UI.PopupMenu;
using UnityEngine;

namespace Data.UI.Opening
{
    [CreateAssetMenu(fileName = "PopupData", menuName = "UI/Opening/PopupData")]
    public class PopupData : ScriptableObject
    {
        public PopupType[] popupTypes;
        public string[] popupTitles;

        public string GetTitle(PopupType popupType)
        {
            int idx = (int)popupType;
            if (idx >= popupTitles.Length)
                return string.Empty;

            return popupTitles[idx];
        }
    }
}