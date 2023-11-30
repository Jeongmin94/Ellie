using Assets.Scripts.Managers;

namespace Assets.Scripts.UI.PopupMenu
{
    public class YesMenuButton : MenuButton
    {
        public static readonly string SoundOk = "click4";

        public override void Click()
        {
            SoundManager.Instance.PlaySound(SoundManager.SoundType.UISfx, SoundOk);
            PopupPayload payload = new PopupPayload();
            payload.buttonType = ButtonType.Yes;

            Invoke(payload);
        }
    }
}