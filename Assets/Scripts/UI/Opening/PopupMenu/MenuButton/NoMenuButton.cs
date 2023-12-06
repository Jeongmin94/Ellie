using Assets.Scripts.Managers;
using UnityEngine;

namespace Assets.Scripts.UI.PopupMenu
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

            PopupPayload payload = new PopupPayload();
            payload.buttonType = ButtonType.No;

            Invoke(payload);
        }
    }
}