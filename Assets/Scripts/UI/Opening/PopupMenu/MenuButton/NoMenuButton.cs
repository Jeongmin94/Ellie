using Assets.Scripts.Managers;

namespace Assets.Scripts.UI.PopupMenu
{
    public class NoMenuButton : MenuButton
    {
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
            PopupPayload payload = new PopupPayload();
            payload.buttonType = ButtonType.No;

            Invoke(payload);
        }
    }
}