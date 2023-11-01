using Assets.Scripts.Managers;
using Assets.Scripts.UI.Framework;
using Assets.Scripts.UI.Framework.Presets;
using Assets.Scripts.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Item.PopupInven
{
    public class HorizontalGridArea : UIBase
    {
        private RectTransform rectTransform;
        private HorizontalLayoutGroup gridLayoutGroup;

        private void Awake()
        {
            Init();
        }

        protected override void Init()
        {
            rectTransform = gameObject.GetOrAddComponent<RectTransform>();
            gridLayoutGroup = gameObject.GetOrAddComponent<HorizontalLayoutGroup>();
        }

        public void InitGridArea()
        {
            AnchorPresets.SetAnchorPreset(rectTransform, AnchorPresets.StretchAll);
            rectTransform.sizeDelta = Vector2.zero;
            rectTransform.localPosition = Vector2.zero;
        }

        public void SetHorizontalGrid(TextAnchor textAnchor, bool expandWidth, bool expandHeight)
        {
            gridLayoutGroup.childAlignment = textAnchor;
            gridLayoutGroup.childForceExpandWidth = expandWidth;
            gridLayoutGroup.childForceExpandHeight = expandHeight;
        }

        public void CreateSlot(int count)
        {
            for (int i = 0; i < count; i++)
            {
                var slot = UIManager.Instance.MakeSubItem<Slot>(rectTransform, UIManager.UISlot);
            }
        }
    }
}