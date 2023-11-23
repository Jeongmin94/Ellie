using Assets.Scripts.UI.Framework;
using Assets.Scripts.Utils;
using Data.UI.Opening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Opening
{
    public class BaseMenuButton : OpeningText
    {
        public new static readonly string Path = "Opening/MenuText";

        private Button menuButton;

        protected override void InitObjects()
        {
            base.InitObjects();

            menuButton = imagePanel.GetComponent<Button>();
        }

        protected override void BindEvents()
        {
            imagePanel.BindEvent(OnClickButton);
        }

        public override void InitTypography(TextTypographyData typographyTypographyData)
        {
            base.InitTypography(typographyTypographyData);

            var colorBlock = menuButton.colors;
            colorBlock.highlightedColor = typographyTypographyData.highlightedColor;
            colorBlock.pressedColor = typographyTypographyData.pressedColor;
            colorBlock.selectedColor = typographyTypographyData.selectedColor;
            colorBlock.disabledColor = typographyTypographyData.disabledColor;

            menuButton.colors = colorBlock;
        }

        protected virtual void OnClickButton(PointerEventData data)
        {
            //!TODO 버튼 클릭 동작 구현
            Debug.Log($"{name} 버튼 클릭됨");
        }
    }
}