using Assets.Scripts.Managers;
using Assets.Scripts.UI.Framework.Popup;
using Assets.Scripts.UI.Player;
using UnityEngine;

namespace Assets.Scripts.UI.Framework
{
    public class UITestClient : MonoBehaviour
    {
        private const string UIButtonCanvas = "ButtonCanvas";
        private const string UIPlayerHealthCanvas = "PlayerHealthCanvas";
        
        private void Start()
        {
            UIManager.Instance.MakePopup<UIPopupButton>(UIButtonCanvas);

            UIManager.Instance.MakeStatic<UIPlayerHealth>(UIPlayerHealthCanvas);
        }
    }
}