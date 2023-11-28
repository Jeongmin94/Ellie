namespace Assets.Scripts.UI.PopupMenu
{
    public class YesMenuButton : MenuButton
    {
        public override void Click()
        {
            PopupPayload payload = new PopupPayload();
            payload.buttonType = ButtonType.Yes;

            Invoke(payload);
        }
    }
}