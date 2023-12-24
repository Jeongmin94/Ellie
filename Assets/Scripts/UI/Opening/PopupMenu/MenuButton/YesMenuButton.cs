using Assets.Scripts.Managers;
using UI.Opening.PopupMenu.PopupCanvas;
using UnityEngine;

namespace UI.Opening.PopupMenu.MenuButton
{
    public class YesMenuButton : MenuButton
    {
        public static readonly string SoundOk = "click4";

        public override void Click()
        {
            SoundManager.Instance.PlaySound(SoundManager.SoundType.Sfx, SoundOk, Vector3.zero);
            var payload = new PopupPayload();
            payload.buttonType = ButtonType.Yes;

            Invoke(payload);
        }
    }
}