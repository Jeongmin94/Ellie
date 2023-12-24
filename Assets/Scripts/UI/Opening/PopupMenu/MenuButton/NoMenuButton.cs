using Assets.Scripts.Managers;
using UI.Opening.PopupMenu.PopupCanvas;
using UnityEngine;

namespace UI.Opening.PopupMenu.MenuButton
{
    public class NoMenuButton : MenuButton
    {
        private static readonly string SoundCancel = "click3";

        private void Awake()
        {
            InputManager.Instance.Subscribe(InputType.Escape, OnEscapeAction);
        }

        private void OnEscapeAction()
        {
            if (gameObject.activeSelf)
            {
                Click();
            }
        }

        public override void Click()
        {
            SoundManager.Instance.PlaySound(SoundManager.SoundType.Sfx, SoundCancel, Vector3.zero);

            var payload = new PopupPayload();
            payload.buttonType = ButtonType.No;

            Invoke(payload);
        }
    }
}