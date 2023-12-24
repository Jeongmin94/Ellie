using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Utils;

namespace UI.Framework.Popup
{
    public class UIPopupButton : UIPopup
    {
        private int score;

        private void Awake()
        {
            Init();
        }

        protected override void Init()
        {
            base.Init();

            Bind<Button>(typeof(UIButtons));
            Bind<TextMeshProUGUI>(typeof(UITexts));

            var btn = GetButton((int)UIButtons.Button);
            var text = GetText((int)UITexts.Text);

            btn.gameObject.BindEvent(OnButtonClicked);
            btn.gameObject.BindEvent(data => { text.text = score++.ToString(); });
        }

        private void OnButtonClicked(PointerEventData data)
        {
            Debug.Log("Button 클릭됨!");
        }
    }
}