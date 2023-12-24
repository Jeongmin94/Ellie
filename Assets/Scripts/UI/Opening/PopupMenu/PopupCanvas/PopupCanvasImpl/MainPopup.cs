using Assets.Scripts.Centers;
using UI.Opening.PopupMenu.MenuButton;

namespace UI.Opening.PopupMenu.PopupCanvas.PopupCanvasImpl
{
    public class MainPopup : PopupCanvas
    {
        public override void Invoke(PopupPayload payload)
        {
            if (payload.buttonType == ButtonType.Yes)
            {
                SceneLoadManager.Instance.LoadScene(SceneName.Opening);
            }
            else
            {
                base.Invoke(payload);
            }
        }
    }
}