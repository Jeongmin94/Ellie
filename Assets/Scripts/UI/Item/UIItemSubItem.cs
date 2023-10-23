using Assets.Scripts.UI.Framework;
using Assets.Scripts.Utils;
using TMPro;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Item
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