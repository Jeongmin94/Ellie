using TMPro;
using UI.Framework.Popup;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Framework.Test
{
    public class UITest : UIPopup
    {
        private void Awake()
        {
            Debug.Log("[UITest] Awake()");
            Init();
        }

        private void Start()
        {
            Debug.Log("[UITest] Start()");
        }

        protected override void Init()
        {
            base.Init();

            Bind<Button>(typeof(Buttons));
            Bind<TextMeshProUGUI>(typeof(Texts));
            Bind<Image>(typeof(Images));
        }

        private enum Buttons
        {
            TestButton
        }

        private enum Texts
        {
            TestText
        }

        private enum Images
        {
            TestImage
        }
    }
}