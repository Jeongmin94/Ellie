using Assets.Scripts.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Framework.Popup
{
    public class UIButton : UIPopup
    {
        private int score = 0;
        
        private void Start()
        {
            Init();
        }

        public override void Init()
        {
            base.Init();

            Bind<Button>(typeof(UIButtons));
            Bind<TextMeshProUGUI>(typeof(UITexts));

            var btn = GetButton((int)UIButtons.Button);
            var text = GetText((int)UITexts.Text);

            btn.gameObject.BindEvent(OnButtonClicked, UIEvent.Click);
            btn.gameObject.BindEvent(data =>
            {
                text.text = score++.ToString();
            }, UIEvent.Click);
        }

        private void OnButtonClicked(PointerEventData data)
        {
            Debug.Log($"Button 클릭됨!");
        }
    }
}