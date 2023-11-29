using System;
using Assets.Scripts.Data.UI.Transform;
using Assets.Scripts.UI.Framework;
using Assets.Scripts.UI.Inventory;
using Assets.Scripts.Utils;
using Data.UI.Config;
using Data.UI.Opening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.UI.PopupMenu
{
    public class ConfigComponent : UIBase
    {
        public static readonly string Path = "Opening/ConfigComponent";

        private enum GameObjects
        {
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

        private Image componentImage;
        private Color imageColor;

        private Action<int> componentAction;

        private void Awake()
        {
            Init();
        }

        protected override void Init()
        {
            Bind();
            InitObjects();
            BindEvents();
        }

        private void Bind()
        {
            Bind<GameObject>(typeof(GameObjects));
            Bind<TextMeshProUGUI>(typeof(Texts));

            optionPrev = GetGameObject((int)GameObjects.OptionPrev);
            optionNext = GetGameObject((int)GameObjects.OptionNext);

            nameText = GetText((int)Texts.NameText);
            optionValue = GetText((int)Texts.OptionValue);

            componentImage = gameObject.GetComponent<Image>();

            rect = gameObject.GetComponent<RectTransform>();
        }

        private void InitObjects()
        {
            rect.sizeDelta = transformData.actionRect.Value.GetSize();
            rect.localPosition = transformData.actionRect.Value.ToCanvasPos();

            var left = optionPrev.GetOrAddComponent<OptionButton>();
            left.InitOptionButton(-1);
            left.Subscribe(OnButtonClick);

            var right = optionNext.GetOrAddComponent<OptionButton>();
            right.InitOptionButton(1);
            right.Subscribe(OnButtonClick);

            imageColor = componentImage.color;

            SetTypography(nameText, typographyData);
            SetTypography(optionValue, typographyData);
        }

        private void BindEvents()
        {
            gameObject.BindEvent(OnPointerEnter, UIEvent.PointEnter);
            gameObject.BindEvent(OnPointerExit, UIEvent.PointExit);
        }

        private void OnPointerEnter(PointerEventData data)
        {
            imageColor.a = 1.0f;
            componentImage.color = imageColor;
        }

        private void OnPointerExit(PointerEventData data)
        {
            imageColor.a = 0.0f;
            componentImage.color = imageColor;
        }

        private void SetTypography(TextMeshProUGUI tmp, TextTypographyData data)
        {
            tmp.font = data.fontAsset;
            tmp.fontSize = data.fontSize;
            tmp.color = data.color;
            tmp.alignment = data.alignmentOptions;
            tmp.lineSpacing = data.lineSpacing;
        }

        public void SetConfigData(string configName,
                                  bool readOnly,
                                  Action<int> onIndexChanged)
        {
            optionPrev.SetActive(!readOnly);
            optionNext.SetActive(!readOnly);

            nameText.text = configName;

            componentAction -= onIndexChanged;
            componentAction += onIndexChanged;
        }

        private void OnButtonClick(int value)
        {
            componentAction?.Invoke(value);
        }

        public void OnOptionValueChanged(string value)
        {
            optionValue.text = value;
        }

        private void OnDestroy()
        {
            componentAction = null;
        }
    }
}