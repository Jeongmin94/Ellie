using Assets.Scripts.Data.UI.Transform;
using Assets.Scripts.UI.Framework;
using Assets.Scripts.UI.Inventory;
using Data.UI.Opening;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI.PopupMenu
{
    public class ConfigComponent : UIBase
    {
        public static readonly string Path = "Opening/ConfigComponent";

        private enum GameObjects
        {
            NamePanel,
            OptionPanel,
            OptionPrev,
            OptionNext,
        }

        private enum Texts
        {
            NameText,
            OptionValue,
        }

        [SerializeField] private UITransformData transformData;
        [SerializeField] private TextTypographyData typographyData;

        private GameObject optionPrev;
        private GameObject optionNext;

        private RectTransform rect;

        private TextMeshProUGUI nameText;
        private TextMeshProUGUI optionValue;

        private void Awake()
        {
            Init();
        }

        protected override void Init()
        {
            Bind();
            InitObjects();
        }

        private void Bind()
        {
            Bind<GameObject>(typeof(GameObjects));
            Bind<TextMeshProUGUI>(typeof(Texts));

            optionPrev = GetGameObject((int)GameObjects.OptionPrev);
            optionNext = GetGameObject((int)GameObjects.OptionNext);

            nameText = GetText((int)Texts.NameText);
            optionValue = GetText((int)Texts.OptionValue);

            rect = gameObject.GetComponent<RectTransform>();
        }

        private void InitObjects()
        {
            rect.sizeDelta = transformData.actionRect.Value.GetSize();
            rect.localPosition = transformData.actionRect.Value.ToCanvasPos();

            SetTypography(nameText, typographyData);
            SetTypography(optionValue, typographyData);
        }

        private void SetTypography(TextMeshProUGUI tmp, TextTypographyData data)
        {
            tmp.font = data.fontAsset;
            tmp.fontSize = data.fontSize;
            tmp.color = data.color;
            tmp.alignment = data.alignmentOptions;
            tmp.lineSpacing = data.lineSpacing;
        }
        
        // 1. prev, next 버튼 클릭함
        // 2. ConfigComponent로 이벤트 도착해서 idx 변경함
        // 3. idx 변경 이벤트를 OptionData로 전파함
        // 4. OptionData에서 idx 변경 이벤트 처리함
        private void OnIndexChanged(int value)
        {
            
        }
    }
}