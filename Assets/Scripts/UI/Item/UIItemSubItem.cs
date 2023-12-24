using TMPro;
using UI.Framework;
using UnityEngine.UI;
using Utils;

namespace UI.Item
{
    public class UIItemSubItem : UIBase
    {
        private Image image;
        private TextMeshProUGUI text;

        private void Awake()
        {
            Init();
        }

        protected override void Init()
        {
            image = gameObject.FindChild<Image>();
            text = gameObject.FindChild<TextMeshProUGUI>();
        }
    }
}