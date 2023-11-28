using Assets.Scripts.Managers;
using UnityEngine;
using Input = UnityEngine.Input;

namespace Assets.Scripts.UI.PopupMenu
{
    public class ConfigPopup : PopupCanvas
    {
        private void Awake()
        {
            InputManager.Instance.OnKeyAction -= OnKeyAction;
            InputManager.Instance.OnKeyAction += OnKeyAction;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            InputManager.Instance.OnKeyAction -= OnKeyAction;
        }

        private void OnKeyAction()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                PopupPayload payload = new PopupPayload();
                payload.popupType = PopupType.Config;
                payload.buttonType = ButtonType.No;

                Invoke(payload);
            }
        }
    }
}