namespace Assets.Scripts.UI.PopupMenu
{
    public class NoMenuButton : MenuButton
    {
        public override void Click()
        {
            PopupPayload payload = new PopupPayload();
            payload.buttonType = ButtonType.No;
            
            Invoke(payload);
        }
    }
}