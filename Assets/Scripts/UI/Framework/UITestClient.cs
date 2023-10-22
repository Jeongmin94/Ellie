using Assets.Scripts.Managers;
using Assets.Scripts.UI.Framework.Popup;
using UnityEngine;

namespace Assets.Scripts.UI.Framework
{
    public class UITestClient : MonoBehaviour
    {
        private const string UIButtonCanvas = "ButtonCanvas";
        
        private void Start()
        {
            UIManager.Instance.MakePopup<UIPopupButton>(UIButtonCanvas);
            UIManager.Instance.MakePopup<UIPopupButton>(UIButtonCanvas);
        }
    }
}