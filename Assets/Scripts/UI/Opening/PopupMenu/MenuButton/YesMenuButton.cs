using Assets.Scripts.Managers;
using UnityEngine;

namespace Assets.Scripts.UI.PopupMenu
{
    public class YesMenuButton : MenuButton
    {
        public static readonly string SoundOk = "click4";

        public override void Click()
        {
            SoundManager.Instance.PlaySound(SoundManager.SoundType.Sfx, SoundOk, Vector3.zero);
            PopupPayload payload = new PopupPayload();
            payload.buttonType = ButtonType.Yes;

            Invoke(payload);
        }
    }
}