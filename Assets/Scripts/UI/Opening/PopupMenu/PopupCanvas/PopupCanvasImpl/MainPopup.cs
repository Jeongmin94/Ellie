using Assets.Scripts.Centers;

namespace Assets.Scripts.UI.PopupMenu
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